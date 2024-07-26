using BookingPlatform.Application.Features.Amenity.Commands.CreateAmenity;
using BookingPlatform.Application.Features.Amenity.Dtos;
using BookingPlatform.Application.Features.Amenity.Queries.GetAmenitiesByName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ApiControllerBase
    {

        [HttpGet("{name}")]
        public async Task<ActionResult<GetAmenitiesByNameResponse>> GetByName(string name)
        {
            var result = await Mediator.Send(new GetHotelImagesByhotelIdQuery()  { Name = name });
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<CreateAmenityResponse>> Create(AmenityDto amenity)
        {
            var result = await Mediator.Send(new CreateAmenityCommand() { CreateAmenity = amenity });
            return result;
        }
    }
}
