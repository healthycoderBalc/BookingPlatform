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

            CreateMap<(Domain.Entities.City City, int NumberOfVisits), TrendingCityDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.City.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.City.ThumbnailUrl))
                .ForMember(dest => dest.NumberOfVisits, opt => opt.MapFrom(src => src.NumberOfVisits));
        }
    }
}
