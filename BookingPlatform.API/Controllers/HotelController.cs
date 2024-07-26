using BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using BookingPlatform.Application.Features.Hotel.Queries.GetRecentHotelsByUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetHotelsBySearchResponse>> GetHotelsBySearch(string? hotelName, [FromQuery] string? cityName, DateTime? checkIn, DateTime? checkOut, int? adults, int? children)
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

        [Authorize]
        [HttpGet("recent")]
        public async Task<ActionResult<GetRecentHotelsByUserResponse>> GetRecentHotels()
        {
            var response = await Mediator.Send(new GetRecentHotelsByUserQuery());
            return response;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<FilterHotelsSearchResponse>> FilterHotels(
            FilterHotelsSearchQuery filterHotelsSearchQuery)
        {
            var response = await Mediator.Send(filterHotelsSearchQuery);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelByIdResponse>> GetById(int id)
        {
            var result = await Mediator.Send(new GetHotelByIdQuery() { Id = id });
            return result;
        }
    }
}
