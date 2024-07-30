using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.FeaturedHotel.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using BookingPlatform.Application.Features.HotelImages.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId
{
    public class GetHotelImagesByHotelIdQueryHandler : IRequestHandler<GetHotelImagesByHotelIdQuery, GetHotelImagesByHotelIdResponse>
    {
        private readonly IHotelImageRepository _repository;
        private readonly IRepository<Domain.Entities.Hotel> _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetHotelImagesByHotelIdQueryHandler(IHotelImageRepository repository, IRepository<Domain.Entities.Hotel> hotelRepository, IMapper mapper, ILogger<GetHotelImagesByHotelIdQueryHandler> logger)
        {
            _repository = repository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<GetHotelImagesByHotelIdResponse> Handle(GetHotelImagesByHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelImagesResponse = new GetHotelImagesByHotelIdResponse();
            var validator = new GetHotelImagesByHotelIdValidator(_hotelRepository);
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    hotelImagesResponse.Success = false;
                    hotelImagesResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return hotelImagesResponse;
                }
                else if (hotelImagesResponse.Success)
                {
                    var result = await _repository.GetByHotelIdAsync(request.HotelId);
                    hotelImagesResponse.HotelImages = _mapper.Map<ICollection<HotelImageDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                hotelImagesResponse.Success = false;
                hotelImagesResponse.Message = ex.Message;
            }

            return hotelImagesResponse;
        }
    }
}
