using BookingPlatform.Application.Features.City.Commands.CreateCity;
using BookingPlatform.Application.Features.City.Commands.UpdateCity;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter;
using BookingPlatform.Application.Features.Hotel.Commands.CreateHotel;
using BookingPlatform.Application.Features.Hotel.Commands.DeleteHotel;
using BookingPlatform.Application.Features.Hotel.Commands.UpdateHotel;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotels;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin;
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetHotelsResponse>> GetHotelsAdmin()
        {
            var response = await Mediator.Send(new GetHotelsQuery());
            return response;
        }


        [HttpGet("search")]
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteHotelResponse>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteHotelCommand() { Id = id });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateHotelResponse>> Create(HotelCreationDto hotel)
        {
            var result = await Mediator.Send(new CreateHotelCommand() { CreateHotel = hotel });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateHotelResponse>> Update(int id, HotelUpdateDto city)
        {
            var result = await Mediator.Send(new UpdateHotelCommand() { Id = id, UpdateHotel = city });
            return result;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-filter")]
        public async Task<ActionResult<GetHotelsByFilterAdminResponse>> GetHotelsByFilterAdmin(string? name, int? starRating, string? ownerName, int? numberOfRooms, DateTime? creationDate, DateTime? modificationDate)
        {
            var response = await Mediator.Send(
                new GetHotelsByFilterAdminQuery() { HotelFilter = new HotelFilterDto(name, starRating, ownerName, numberOfRooms, creationDate, modificationDate) });
            return response;
        }
    }
}
