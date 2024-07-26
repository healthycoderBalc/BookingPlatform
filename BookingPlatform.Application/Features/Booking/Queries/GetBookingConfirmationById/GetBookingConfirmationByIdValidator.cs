using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById
{
    public class GetBookingConfirmationByIdValidator : AbstractValidator<GetBookingConfirmationByIdQuery>
    {
        public GetBookingConfirmationByIdValidator()
        {
            
        }
    }
}
