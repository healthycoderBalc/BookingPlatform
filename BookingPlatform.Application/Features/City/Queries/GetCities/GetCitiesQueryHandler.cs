﻿using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCities
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, GetCitiesResponse>
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCitiesQueryHandler> _logger;

        public GetCitiesQueryHandler(ICityRepository repository, IMapper mapper, ILogger<GetCitiesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetCitiesResponse> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var getCityResponse = new GetCitiesResponse();
            var validator = new GetCitiesValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count >0)
                {
                    getCityResponse.Success = false;
                    getCityResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return getCityResponse;
                }
                else if (getCityResponse.Success)
                {
                    var result = await _repository.GetCitiesAdminAsync();
                    getCityResponse.CitiesAdmin = _mapper.Map<ICollection<CityAdminDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getCityResponse.Success = false;
                getCityResponse.Message = ex.Message;
            }

            return getCityResponse;
        }
    }
}
