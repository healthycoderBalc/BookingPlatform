using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.CreateCity
{
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CreateCityResponse>
    {
        private readonly IRepository<Domain.Entities.City> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateCityCommandHandler(IRepository<Domain.Entities.City> repository, IMapper mapper, ILogger<CreateCityCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CreateCityResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var createCityResponse = new CreateCityResponse();
            var validator = new CreateCityValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    createCityResponse.Success = false;
                    createCityResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        createCityResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (createCityResponse.Success)
                {
                    var cityEntity = _mapper.Map<Domain.Entities.City>(request.CreateCity);
                    var result = await _repository.AddAsync(cityEntity);
                    createCityResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                createCityResponse.Success = false;
                createCityResponse.Message = ex.Message;
            }

            return createCityResponse;


        }
    }
}
