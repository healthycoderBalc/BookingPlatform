using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById
{
    public class GetBookingConfirmationByIdQuery : IRequest<GetBookingConfirmationByIdResponse>
    {
        public int Id { get; set; }
    }
}
