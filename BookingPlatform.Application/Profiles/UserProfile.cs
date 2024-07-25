using AutoMapper;
using AutoMapper.Features;
using BookingPlatform.Application.Features.User.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.Entities.User, UserForRegistrationDto>()
                .ReverseMap();
            CreateMap<Domain.Entities.User, UserForLoginDto>()
              .ReverseMap();
            CreateMap<Domain.Entities.User, UserDto>()
              .ReverseMap();

        }
    }
}
