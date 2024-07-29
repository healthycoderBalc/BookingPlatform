using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Response;

namespace BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels
{
    public class GetFeaturedHotelsResponse :BaseResponse
    {
        public ICollection<FeaturedHotelDto> FeaturedHotels { get; set; }
    }
}
