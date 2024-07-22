using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Domain.Entities.City, CityDto>()
                .ReverseMap();
        }
    }
}
