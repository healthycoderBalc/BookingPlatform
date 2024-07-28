using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Room.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin
{
    public class GetRoomsByFilterAdminQuery : IRequest<GetRoomsByFilterAdminResponse>
    {
        public RoomFilterDto RoomFilter {  get; set; }
    }
}
