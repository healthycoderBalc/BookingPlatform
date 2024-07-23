using BookingPlatform.Application.Features.Amenity.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Amenity.Queries.GetAmenitiesByName
{
    public class GetAmenitiesByNameResponse : BaseResponse
    {
        public ICollection<AmenityDto> Amenities { get; set; }
    }
}
