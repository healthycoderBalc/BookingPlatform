using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, DetailedRoomDto>()
                .ReverseMap();

            CreateMap<RoomImage, RoomImageDto>()
                .ReverseMap();
        }
    }
}
