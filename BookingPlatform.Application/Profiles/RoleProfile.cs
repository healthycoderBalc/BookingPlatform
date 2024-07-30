using AutoMapper;
using BookingPlatform.Application.Features.Role.Dtos;
using BookingPlatform.Application.Features.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Domain.Entities.Role, RoleDto>()
               .ReverseMap();
        }
    }
}
