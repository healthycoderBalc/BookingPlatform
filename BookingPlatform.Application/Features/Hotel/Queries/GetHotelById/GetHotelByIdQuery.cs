using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdQuery : IRequest<GetHotelByIdResponse>
    {
        public int Id { get; set; }
    }
}
