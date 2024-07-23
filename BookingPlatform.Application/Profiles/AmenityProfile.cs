using AutoMapper;
using BookingPlatform.Application.Features.Amenity.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class AmenityProfile : Profile
    {
        public AmenityProfile()
        {
            CreateMap<Domain.Entities.Amenity, AmenityDto>()
                .ReverseMap();
        }
    }
}
