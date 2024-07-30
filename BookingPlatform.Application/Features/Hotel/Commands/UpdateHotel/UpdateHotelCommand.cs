using BookingPlatform.Application.Features.Hotel.Dtos;
using MediatR;

namespace BookingPlatform.Application.Features.Hotel.Commands.UpdateHotel
{
    public class UpdateHotelCommand : BaseCommandQuery, IRequest<UpdateHotelResponse>
    {
        public HotelUpdateDto UpdateHotel { get; set; }
    }
}
