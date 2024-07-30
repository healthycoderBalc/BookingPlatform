using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter
{
    public class GetCitiesByFilterResponse : BaseResponse
    {
        public ICollection<CityAdminDto> FilteredCities { get; set; }
    }
}
