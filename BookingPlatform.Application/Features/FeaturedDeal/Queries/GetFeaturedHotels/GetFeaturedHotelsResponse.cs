using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels
{
    public class GetFeaturedHotelsResponse :BaseResponse
    {
        public ICollection<FeaturedHotelDto> FeaturedHotels { get; set; }
    }
}
