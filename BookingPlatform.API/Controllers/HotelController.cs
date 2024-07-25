using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetHotelsBySearchResponse>> GetHotelsBySearch(string? hotelName, string? cityName, DateTime? checkIn, DateTime? checkOut, int? adults, int? children)
        {
            var response = await Mediator.Send(
                new GetHotelsBySearchQuery()
                {
                    HotelName = hotelName,
                    CityName = cityName,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    Adults = adults,
                    Children = children
                });
            return response;
        }
    }
}
