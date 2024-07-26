using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.HotelImages.Dtos;
using BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, GetHotelByIdResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetHotelByIdQueryHandler(IHotelRepository repository, IMapper mapper, ILogger<GetHotelByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetHotelByIdResponse> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotelResponse = new GetHotelByIdResponse();
            var validator = new GetHotelByIdValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
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
                    var result = await _repository.GetByIdAsync(request.Id);
                    hotelResponse.DetailedHotel = _mapper.Map<DetailedHotelDto>(result);
                    hotelResponse.HotelReviews = _mapper.Map<ICollection<HotelReviewDto>>(result.HotelReviews);
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
