using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.City.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Commands.AddToCart
{
    public class AddToCartCommand : IRequest<AddToCartResponse>
    {
        public AddToCartBookingDto AddToCartBooking { get; set; }
    }
}
