using AutoMapper;
using BookingPlatform.Application.Features.Amenity.Dtos;
using BookingPlatform.Application.Features.HotelImages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
