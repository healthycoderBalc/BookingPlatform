using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCityById
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, GetCityByIdResponse>
    {
        private readonly IRepository<Domain.Entities.City> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetCityByIdQueryHandler(IRepository<Domain.Entities.City> repository, IMapper mapper, ILogger<GetCityByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetCityByIdResponse> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var cityResponse = new GetCityByIdResponse();
            var validator = new GetCityByIdValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    cityResponse.Success = false;
                    cityResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        cityResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (cityResponse.Success)
                {
                    var result = await _repository.GetByIdAsync(request.Id);
                    cityResponse.City = _mapper.Map<CityAdminDto>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                cityResponse.Success = false;
                cityResponse.Message = ex.Message;
            }

            return cityResponse;
        }
    }
}
