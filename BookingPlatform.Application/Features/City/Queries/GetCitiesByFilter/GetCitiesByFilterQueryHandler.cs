using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter
{
    public class GetCitiesByFilterQueryHandler : IRequestHandler<GetCitiesByFilterQuery, GetCitiesByFilterResponse>
    {

        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetCitiesByFilterQueryHandler(ICityRepository repository, IMapper mapper, ILogger<GetCitiesByFilterQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCitiesByFilterResponse> Handle(GetCitiesByFilterQuery request, CancellationToken cancellationToken)
        {
            var cityResponse = new GetCitiesByFilterResponse();
            var validator = new GetCitiesByFilterValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    cityResponse.Success = false;
                    cityResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return cityResponse;
                }
                else if (cityResponse.Success)
                {
                    var result = await _repository.GetCitiesByFilterAsync(request.CityFilter.Name, request.CityFilter.Country, request.CityFilter.PostalCode, request.CityFilter.NumberOfHotels, request.CityFilter.CreationDate, request.CityFilter.ModificationDate);
                    cityResponse.FilteredCities = _mapper.Map<ICollection<CityAdminDto>>(result);
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
