using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf
{
    public class GetBookingPdfValidator : AbstractValidator<GetBookingPdfQuery>
    {
        public GetBookingPdfValidator()
        {
            
        }
    }
}
