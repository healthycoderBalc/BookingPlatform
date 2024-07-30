using AutoMapper;
using BookingPlatform.Application.Features.HotelImages.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class HotelImageProfile : Profile
    {
        public HotelImageProfile()
        {
            CreateMap<Domain.Entities.HotelImage, HotelImageDto>()
              .ReverseMap();
        }
    }
}
