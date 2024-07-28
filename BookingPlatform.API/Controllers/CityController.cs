using BookingPlatform.Application.Features.City.Commands.CreateCity;
using BookingPlatform.Application.Features.City.Commands.DeleteCity;
using BookingPlatform.Application.Features.City.Commands.UpdateCity;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter;
using BookingPlatform.Application.Features.City.Queries.GetCityById;
using BookingPlatform.Application.Features.City.Queries.GetTrendingCities;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ApiControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetCitiesResponse>> GetCitiesAdmin()
        {
            var response = await Mediator.Send(new GetCitiesQuery());
            return response;
        }

        [HttpGet("trending")]
        public async Task<ActionResult<GetTrendingCitiesResponse>> GetTrendingCities()
        {
            var response = await Mediator.Send(new GetTrendingCitiesQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCityByIdResponse>> GetById(int id)
        {
            var result = await Mediator.Send(new GetCityByIdQuery() { Id = id });
            return result;
        }

        [Authorize (Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateCityResponse>> Create(CityCreationDto city)
        {
            var result = await Mediator.Send(new CreateCityCommand() { CreateCity = city });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCityResponse>> Update(int id, CityUpdateDto city)
        {
            var result = await Mediator.Send(new UpdateCityCommand() { Id = id, UpdateCity = city });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteCityResponse>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteCityCommand() { Id = id });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("filter")]
        public async Task<ActionResult<GetCitiesByFilterResponse>> GetCitiesByFilter(string? name, string? country, string? postalCode, int? numberOfHotels, DateTime? creationDate, DateTime? modificationDate)
        {
            var response = await Mediator.Send(
                new GetCitiesByFilterQuery() { CityFilter = new CityFilterDto(name, country, postalCode, numberOfHotels, creationDate, modificationDate) });
            return response;
        }
    }
}
