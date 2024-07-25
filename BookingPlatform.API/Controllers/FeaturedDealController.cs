using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedDealController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetFeaturedHotelsResponse>> GetFeaturedHotels()
        {
            var response = await Mediator.Send(new GetFeaturedHotelsQuery());
            return response;
        }
    }
}
