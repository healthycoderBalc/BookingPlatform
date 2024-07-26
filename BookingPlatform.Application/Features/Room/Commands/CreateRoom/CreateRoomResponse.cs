using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomResponse : BaseResponse
    {
        public int Id { get; set; }
    }
}
