using BookingPlatform.Application.Features.City.Commands.CreateCity;
using BookingPlatform.Application.Features.City.Commands.DeleteCity;
using BookingPlatform.Application.Features.City.Commands.UpdateCity;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Features.City.Queries.GetCityById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetCitiesResponse>> GetCities()
        {
            var response = await Mediator.Send(new GetCitiesQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCityByIdResponse>> GetById(int id)
        {
            var result = await Mediator.Send(new GetCityByIdQuery() { Id = id });
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<CreateCityResponse>> Create(CityDto city)
        {
            var result = await Mediator.Send(new CreateCityCommand() { CreateCity = city });
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCityResponse>> Update(int id, CityDto city)
        {
            var result = await Mediator.Send(new UpdateCityCommand() { Id = id, UpdateCity = city });
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteCityResponse>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteCityCommand() { Id = id });
            return result;
        }
    }
}
