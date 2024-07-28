using BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelImageController : ApiControllerBase
    {
        [HttpGet("{hotelId}/images")]
        public async Task<ActionResult<GetHotelImagesByHotelIdResponse>> GetByHotelId(int hotelId)
        {
            var result = await Mediator.Send(new GetHotelImagesByHotelIdQuery() { HotelId = hotelId});
            return result;
        }
    }
}
