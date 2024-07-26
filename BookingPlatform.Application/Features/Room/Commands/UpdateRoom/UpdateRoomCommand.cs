using BookingPlatform.Application.Features.Room.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommand : BaseCommandQuery, IRequest<UpdateRoomResponse>
    {
        public RoomUpdateDto RoomUpdate {  get; set; }
    }
}
