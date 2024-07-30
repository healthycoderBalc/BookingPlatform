using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotels
{
    public class GetHotelsResponse : BaseResponse
    {
        public ICollection<HotelAdminDto> HotelsAdmin { get; set; }
    }
}
