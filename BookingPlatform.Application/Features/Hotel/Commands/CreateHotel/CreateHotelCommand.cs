using BookingPlatform.Application.Features.Hotel.Dtos;
using MediatR;

namespace BookingPlatform.Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommand : IRequest<CreateHotelResponse>
    {
        public HotelCreationDto CreateHotel { get; set; }

    }
}
