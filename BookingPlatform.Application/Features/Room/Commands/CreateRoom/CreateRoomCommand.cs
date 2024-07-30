using BookingPlatform.Application.Features.Room.Dtos;
using MediatR;

namespace BookingPlatform.Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomCommand: IRequest<CreateRoomResponse>
    {
        public RoomCreationDto CreateRoom { get; set; }
    }
}
