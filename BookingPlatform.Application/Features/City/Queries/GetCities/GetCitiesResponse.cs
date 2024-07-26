using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCities
{
    public class GetCitiesResponse : BaseResponse
    {
        public ICollection<CityAdminDto> CitiesAdmin { get; set; }
    }
}
