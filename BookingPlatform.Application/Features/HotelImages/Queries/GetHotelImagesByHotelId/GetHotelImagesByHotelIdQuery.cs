using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId
{
    public class GetHotelImagesByHotelIdQuery : IRequest<GetHotelImagesByHotelIdResponse>
    {
        public int HotelId { get; set; }
    }
}
