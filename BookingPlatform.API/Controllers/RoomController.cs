﻿using BookingPlatform.Application.Features.Room.Commands.CreateRoom;
using BookingPlatform.Application.Features.Room.Commands.DeleteRoom;
using BookingPlatform.Application.Features.Room.Commands.UpdateRoom;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRooms;
using BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ApiControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetRoomsResponse>> GetRoomsAdmin()
        {
            var response = await Mediator.Send(new GetRoomsQuery());
            return response;
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult<GetRoomsByHotelIdResponse>> GetByHotelId(int hotelId)
        {
            var result = await Mediator.Send(new GetRoomsByHotelIdQuery() { HotelId = hotelId });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteRoomResponse>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteRoomCommand() { Id = id });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateRoomResponse>> Create(RoomCreationDto city)
        {
            var result = await Mediator.Send(new CreateRoomCommand() { CreateRoom = city });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateRoomResponse>> Update(int id, RoomUpdateDto city)
        {
            var result = await Mediator.Send(new UpdateRoomCommand() { Id = id, RoomUpdate = city });
            return result;
        }
    }
}