using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId
{
    public class GetRoomsByHotelIdQuery : IRequest<GetRoomsByHotelIdResponse>
    {
        public int HotelId { get; set; }
    }
}
