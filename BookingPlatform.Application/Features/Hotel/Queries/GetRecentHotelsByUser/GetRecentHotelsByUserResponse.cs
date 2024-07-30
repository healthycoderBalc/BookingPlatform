using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetRecentHotelsByUser
{
    public class GetRecentHotelsByUserResponse : BaseResponse
    {
        public ICollection<RecentHotelDto> RecentHotels {  get; set; }
    }
}
