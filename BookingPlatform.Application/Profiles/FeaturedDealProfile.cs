using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Profiles
{
    public class FeaturedDealProfile : Profile
    {
        public FeaturedDealProfile()
        {
            CreateMap<Domain.Entities.FeaturedDeal, FeaturedHotelDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Room.Hotel.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Room.Hotel.Name))
               .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Room.Hotel.ThumbnailUrl))
               .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Room.Hotel.ShortDescription))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Room.Hotel.Address))
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Room.Hotel.City.Name))
               .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.Room.PricePerNight))
               .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src => src.DiscountedPricePerNight))
               .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Room.Hotel.StarRating))
               .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.Type.Name));
        }
    }
}
