using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Commands.AddToCart
{
    public class AddToCartValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartValidator()
        {
            RuleFor(c => c.AddToCartBooking.RoomId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(h => h.AddToCartBooking.CheckInDate)
               .NotEmpty()
               .NotNull()
               .WithMessage("CheckIn date must be provided.");

            RuleFor(h => h.AddToCartBooking.CheckOutDate)
                .NotEmpty()
                .NotNull()
                .WithMessage("CheckOut date must be provided");

            RuleFor(h => h)
                .Must(h => h.AddToCartBooking.CheckOutDate <= h.AddToCartBooking.CheckOutDate)
                .WithMessage("CheckIn date must be before or equal to CheckOut date.");

        }
    }
}
