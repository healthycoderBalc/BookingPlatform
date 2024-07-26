using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Commands.CreateHotel;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, CreateHotelResponse>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;
        private readonly IRepository<Domain.Entities.City> _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateHotelCommandHandler(IRepository<Domain.Entities.Hotel> repository, IRepository<Domain.Entities.City> cityRepository, IMapper mapper, ILogger<CreateHotelCommandHandler> logger)
        {
            _repository = repository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<CreateHotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var createHotelResponse = new CreateHotelResponse();
            var validator = new CreateHotelValidator(_cityRepository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    createHotelResponse.Success = false;
                    createHotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return createHotelResponse;
                }
                else if (createHotelResponse.Success)
                {
                    var hotelEntity = _mapper.Map<Domain.Entities.Hotel>(request.CreateHotel);
                 
                    var result = await _repository.AddAsync(hotelEntity);
                    createHotelResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                createHotelResponse.Success = false;
                createHotelResponse.Message = ex.Message;
            }

            return createHotelResponse;
        }
    }
}
