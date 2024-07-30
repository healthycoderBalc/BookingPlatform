using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin
{
    public class GetRoomsByFilterAdminResponse : BaseResponse
    {
        public ICollection<RoomAdminDto> FilteredRooms { get; set; }
    }
}
