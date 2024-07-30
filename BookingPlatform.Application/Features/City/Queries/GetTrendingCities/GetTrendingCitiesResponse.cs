using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetTrendingCities
{
    public class GetTrendingCitiesResponse : BaseResponse
    {
        public ICollection<TrendingCityDto> TrendingCities { get; set; }
    }
}
