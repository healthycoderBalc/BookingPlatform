using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Amenity.Queries.GetAmenitiesByName
{
    public class GetHotelImagesByhotelIdQuery : IRequest<GetAmenitiesByNameResponse>
    {
        public string Name { get; set; }
    }
}
