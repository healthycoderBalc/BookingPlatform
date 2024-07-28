using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin
{
    public class GetHotelsByFilterAdminQueryHandler : IRequestHandler<GetHotelsByFilterAdminQuery, GetHotelsByFilterAdminResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetHotelsByFilterAdminQueryHandler(IHotelRepository repository, IMapper mapper, ILogger<GetHotelsByFilterAdminQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetHotelsByFilterAdminResponse> Handle(GetHotelsByFilterAdminQuery request, CancellationToken cancellationToken)
        {
            var hotelResponse = new GetHotelsByFilterAdminResponse();
            var validator = new GetHotelsByFilterAdminValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    hotelResponse.Success = false;
                    hotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return hotelResponse;
                }
                else if (hotelResponse.Success)
                {
                    var result = await _repository.GetHotelsByFilterAdminAsync(request.HotelFilter.Name, request.HotelFilter.StarRating, request.HotelFilter.OwnerName, request.HotelFilter.NumberOfRooms, request.HotelFilter.CreationDate, request.HotelFilter.ModificationDate);
                    hotelResponse.FilteredHotels = _mapper.Map<ICollection<HotelAdminDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                hotelResponse.Success = false;
                hotelResponse.Message = ex.Message;
            }

            return hotelResponse;
        }
    }
}
