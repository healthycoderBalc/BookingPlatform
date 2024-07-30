using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
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

            CreateMap<Room, RoomAdminDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
               .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.IsOperational))
               .ForMember(dest => dest.AdultCapacity, opt => opt.MapFrom(src => src.AdultCapacity))
               .ForMember(dest => dest.ChildrenCapacity, opt => opt.MapFrom(src => src.ChildrenCapacity))
               .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => src.UpdatedAt));

            CreateMap<Room, RoomCreationDto>()
                .ReverseMap();
            CreateMap<Room, RoomUpdateDto>()
            .ReverseMap();

        }
    }
}
