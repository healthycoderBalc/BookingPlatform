using AutoMapper;
using BookingPlatform.Application.Features.Booking.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Domain.Entities.Booking, AddToCartBookingDto>()
             .ReverseMap();

            CreateMap<Domain.Entities.Booking, BookingConfirmationDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.ConfirmationNumber, opt => opt.MapFrom(src => src.ConfirmationNumber))
              .ForMember(dest => dest.HotelAddress, opt => opt.MapFrom(src => src.Room.Hotel.Address))
              .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
              .ForMember(dest => dest.RoomDescription, opt => opt.MapFrom(src => src.Room.Description))
              .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room.Id))
              .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate))
              .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate))
              .ForMember(dest => dest.NumberOfAdults, opt => opt.MapFrom(src => src.NumberOfAdults))
              .ForMember(dest => dest.NumberOfChildren, opt => opt.MapFrom(src => src.NumberOfChildren))
              .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
              .ForMember(dest => dest.SpecialRequests, opt => opt.MapFrom(src => src.SpecialRequests))
              .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => src.IsConfirmed));
        }
    }
}
