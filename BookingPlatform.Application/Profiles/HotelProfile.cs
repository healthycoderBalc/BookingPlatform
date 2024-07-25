using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Domain.Entities.Hotel, HotelDto>()
               .ReverseMap();
        }
    }
}
