using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch
{
    public class FilterHotelsSearchQueryHandler : IRequestHandler<FilterHotelsSearchQuery, FilterHotelsSearchResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FilterHotelsSearchQueryHandler(IHotelRepository repository, IMapper mapper, ILogger<FilterHotelsSearchQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FilterHotelsSearchResponse> Handle(FilterHotelsSearchQuery request, CancellationToken cancellationToken)
        {
            var hotelFilterResponse = new FilterHotelsSearchResponse();
            var validator = new FilterHotelsSearchValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    hotelFilterResponse.Success = false;
                    hotelFilterResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return hotelFilterResponse;
                }
                else if (hotelFilterResponse.Success)
                {
                    var result = await _repository.FilterHotelsAsync(
                        request.HotelIds,
                        request.MinPrice,
                        request.MaxPrice,
                        request.MinStarRating,
                        request.MaxStarRating,
                        request.Amenities,
                        request.RoomTypes,
                        request.PageNumber,
                        request.PageSize);
                    hotelFilterResponse.FilteredHotels = _mapper.Map<ICollection<FilterHotelDto>>(result.Hotels);
                    hotelFilterResponse.TotalCount = result.TotalCount;
                    hotelFilterResponse.TotalCountThisPage = result.TotalCountThisPage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                hotelFilterResponse.Success = false;
                hotelFilterResponse.Message = ex.Message;
            }

            return hotelFilterResponse;
        }
    }
}
