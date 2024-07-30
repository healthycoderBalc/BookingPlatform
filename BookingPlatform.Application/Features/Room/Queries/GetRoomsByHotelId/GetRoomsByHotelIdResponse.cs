using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId
{
    public class GetRoomsByHotelIdResponse : BaseResponse
    {
        public ICollection<DetailedRoomDto> DetailedRooms { get; set; }
    }
}
