using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch
{
    public class GetHotelsBySearchResponse : BaseResponse
    {
        public ICollection<HotelDto> Hotels { get; set; }
    }
}
