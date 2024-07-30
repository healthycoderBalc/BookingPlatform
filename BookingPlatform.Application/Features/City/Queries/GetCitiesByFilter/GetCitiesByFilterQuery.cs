using BookingPlatform.Application.Features.City.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter
{
    public class GetCitiesByFilterQuery : IRequest<GetCitiesByFilterResponse>
    {
        public CityFilterDto CityFilter {  get; set; }
    }
}
