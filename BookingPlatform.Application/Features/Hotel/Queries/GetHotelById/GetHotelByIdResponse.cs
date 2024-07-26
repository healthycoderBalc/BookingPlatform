using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdResponse : BaseResponse
    {
        public DetailedHotelDto DetailedHotel { get; set; }

        public ICollection<HotelReviewDto> HotelReviews { get; set; }
    }
}
