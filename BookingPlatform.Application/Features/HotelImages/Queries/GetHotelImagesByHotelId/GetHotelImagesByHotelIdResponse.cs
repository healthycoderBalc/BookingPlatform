using BookingPlatform.Application.Features.HotelImages.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId
{
    public class GetHotelImagesByHotelIdResponse : BaseResponse
    {
        public ICollection<HotelImageDto> HotelImages { get; set; }
    }
}
