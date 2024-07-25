using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetRecentHotelsByUser
{
    public class GetRecentHotelsByUserQueryHandler : IRequestHandler<GetRecentHotelsByUserQuery, GetRecentHotelsByUserResponse>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetRecentHotelsByUserQueryHandler(IHotelRepository hotelRepository,IMapper mapper, ILogger<GetRecentHotelsByUserQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetRecentHotelsByUserResponse> Handle(GetRecentHotelsByUserQuery request, CancellationToken cancellationToken)
        {
            var hotelResponse = new GetRecentHotelsByUserResponse();
            var validator = new GetRecentHotelsByUserValidator();
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
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        hotelResponse.Success = false;
                        return hotelResponse;
                    }
                    var recentHotels = await _hotelRepository.GetRecentHotelsByAuthUserAsync(userId);
                    hotelResponse.RecentHotels = _mapper.Map<ICollection<RecentHotelDto>>(recentHotels);
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
