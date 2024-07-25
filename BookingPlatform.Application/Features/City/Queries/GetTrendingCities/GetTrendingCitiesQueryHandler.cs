using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetTrendingCities;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetTrendingCities
{
    public class GetTrendingCitiesQueryHandler : IRequestHandler<GetTrendingCitiesQuery, GetTrendingCitiesResponse>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTrendingCitiesQueryHandler> _logger;

        public GetTrendingCitiesQueryHandler(ICityRepository repository, IMapper mapper, ILogger<GetTrendingCitiesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetTrendingCitiesResponse> Handle(GetTrendingCitiesQuery request, CancellationToken cancellationToken)
        {
            var getTrendingCityResponse = new GetTrendingCitiesResponse();
            var validator = new GetTrendingCitiesValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    getTrendingCityResponse.Success = false;
                    getTrendingCityResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return getTrendingCityResponse;
                }
                else if (getTrendingCityResponse.Success)
                {
                    var result = await _repository.GetTrendingCitiesAsync();
                    getTrendingCityResponse.TrendingCities = _mapper.Map<ICollection<TrendingCityDto>>(result);
                     
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getTrendingCityResponse.Success = false;
                getTrendingCityResponse.Message = ex.Message;
            }

            return getTrendingCityResponse;
        }
    }
}
