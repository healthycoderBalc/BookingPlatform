using BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ApiControllerBase
    {
        [HttpGet("{hotelId}")]
        public async Task<ActionResult<GetRoomsByHotelIdResponse>> GetByHotelId(int hotelId)
        {
            var result = await Mediator.Send(new GetRoomsByHotelIdQuery() { HotelId = hotelId });
            return result;
        }
    }
}
