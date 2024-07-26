﻿using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Domain.Entities.Hotel, HotelDto>()
               .ReverseMap();

            CreateMap<(Hotel, Room), RecentHotelDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Item1.City.Name))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Item2.PricePerNight))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item1.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Item1.ThumbnailUrl))
                .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Item1.StarRating));

            CreateMap<(Hotel Hotel, ICollection<decimal> PricesPerNight), FilterHotelDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Hotel.ThumbnailUrl))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Hotel.ShortDescription))
                .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Hotel.Id))
                .ForMember(dest => dest.PricesPerNight, opt => opt.MapFrom(src => src.PricesPerNight));

            CreateMap<Hotel, DetailedHotelDto>()
              .ReverseMap();

            CreateMap<(Hotel Hotel, int NumberOfRooms), HotelAdminDto>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Hotel.Id))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Hotel.Name))
                        .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
                        .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Hotel.OwnerName))
                        .ForMember(dest => dest.NumberOfRooms, opt => opt.MapFrom(src => src.NumberOfRooms))
                        .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.Hotel.CreatedAt))
                        .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => src.Hotel.UpdatedAt));

            CreateMap<HotelReview, HotelReviewDto>()
                .ReverseMap();

            CreateMap<Hotel, HotelCreationDto>()
                .ReverseMap();

            CreateMap<Hotel, HotelUpdateDto>()
                .ReverseMap();
        }
    }
}
