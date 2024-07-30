using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>()
                .ReverseMap();

            CreateMap<(City City, int NumberOfVisits), TrendingCityDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.City.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.City.ThumbnailUrl))
                .ForMember(dest => dest.NumberOfVisits, opt => opt.MapFrom(src => src.NumberOfVisits));

            CreateMap<(City City, int NumberOfHotels), CityAdminDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.City.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City.Name))
              .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country))
              .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.City.PostalCode))
              .ForMember(dest => dest.NumberOfHotels, opt => opt.MapFrom(src => src.NumberOfHotels))
              .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.City.CreatedAt))
              .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => src.City.UpdatedAt));

            CreateMap<City, CityCreationDto>()
                .ReverseMap();
            CreateMap<City, CityUpdateDto>()
               .ReverseMap();
            CreateMap<City, CityAdminDto>()
                .ReverseMap();
        }
    }
}
