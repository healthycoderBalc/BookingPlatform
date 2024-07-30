using BookingPlatform.Application.Features.Hotel.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin
{
    public class GetHotelsByFilterAdminQuery : IRequest<GetHotelsByFilterAdminResponse>
    {
        public HotelFilterDto HotelFilter { get; set; }
    }
}
