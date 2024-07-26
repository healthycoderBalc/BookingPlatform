using BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelImageController : ApiControllerBase
    {
        [HttpGet("{hotelId}")]
        public async Task<ActionResult<GetHotelImagesByHotelIdResponse>> GetByHotelId(int hotelId)
        {
            var result = await Mediator.Send(new GetHotelImagesByHotelIdQuery() { HotelId = hotelId});
            return result;
        }
    }
}
