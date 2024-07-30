using BookingPlatform.Application.Features.Room.Commands.CreateRoom;
using BookingPlatform.Application.Features.Room.Commands.DeleteRoom;
using BookingPlatform.Application.Features.Room.Commands.UpdateRoom;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRooms;
using BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/rooms")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-filter")]
        public async Task<ActionResult<GetRoomsByFilterAdminResponse>> GetRoomsByFilterAdmin(string? roomNumber, bool? availability, int? adultCapacity, int? childrenCapacity, DateTime? creationDate, DateTime? modificationDate)
        {
            var response = await Mediator.Send(
                new GetRoomsByFilterAdminQuery() { RoomFilter = new RoomFilterDto(roomNumber, availability, adultCapacity, childrenCapacity, creationDate, modificationDate) });
            return response;
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteRoomResponse>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteRoomCommand() { Id = id });
            return result;
        }
    }
}
