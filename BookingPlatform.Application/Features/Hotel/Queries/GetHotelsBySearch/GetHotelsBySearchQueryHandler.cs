using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch
{
    public class GetHotelsBySearchQueryHandler : IRequestHandler<GetHotelsBySearchQuery, GetHotelsBySearchResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetHotelsBySearchQueryHandler(IHotelRepository repository, IMapper mapper, ILogger<GetHotelsBySearchQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetHotelsBySearchResponse> Handle(GetHotelsBySearchQuery request, CancellationToken cancellationToken)
        {
            var hotelResponse = new GetHotelsBySearchResponse();
            var validator = new GetHotelsBySearchValidator();
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
                }
                else if (hotelResponse.Success)
                {
                    var result = await _repository.GetHotelsBySearchAsync(request.HotelName, request.CityName, request.CheckIn, request.CheckOut, request.Adults, request.Children);
                    hotelResponse.Hotels = _mapper.Map<ICollection<HotelDto>>(result);
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
