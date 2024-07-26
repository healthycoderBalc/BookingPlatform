using AutoMapper;
using BookingPlatform.Application.Features.City.Commands.DeleteCity;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Commands.DeleteHotel
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, DeleteHotelResponse>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteHotelCommandHandler(IRepository<Domain.Entities.Hotel> repository, IMapper mapper, ILogger<DeleteHotelCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<DeleteHotelResponse> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var deleteHotelResponse = new DeleteHotelResponse();
            var validator = new DeleteHotelValidator(_repository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    deleteHotelResponse.Success = false;
                    deleteHotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return deleteHotelResponse;
                }
                else if (deleteHotelResponse.Success)
                {
                    var hotelEntity = await _repository.GetByIdAsync(request.Id);
                    await _repository.DeleteAsync(hotelEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                deleteHotelResponse.Success = false;
                deleteHotelResponse.Message = ex.Message;
            }

            return deleteHotelResponse;
        }
    }
}
