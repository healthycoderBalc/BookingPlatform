using MediatR;

namespace BookingPlatform.Application.Features.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommand : BaseCommandQuery, IRequest<DeleteRoomResponse>
    {
    }
}
