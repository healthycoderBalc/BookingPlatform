using AutoMapper;
using BookingPlatform.Application.Features.City.Commands.UpdateCity;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.UpdateCity
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, UpdateCityResponse>
    {
        private readonly IRepository<Domain.Entities.City> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateCityCommandHandler(IRepository<Domain.Entities.City> repository, IMapper mapper, ILogger<UpdateCityCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UpdateCityResponse> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var updateCityResponse = new UpdateCityResponse();
            var validator = new UpdateCityValidator(_repository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    updateCityResponse.Success = false;
                    updateCityResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        updateCityResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (updateCityResponse.Success)
                {
                    var cityEntity = await _repository.GetByIdAsync(request.Id);
                    _mapper.Map(request.UpdateCity, cityEntity);
                    await _repository.UpdateAsync(cityEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                updateCityResponse.Success = false;
                updateCityResponse.Message = ex.Message;
            }

            return updateCityResponse;
        }
    }
}
