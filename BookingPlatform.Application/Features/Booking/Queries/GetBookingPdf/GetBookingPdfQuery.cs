using MediatR;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf
{
    public class GetBookingPdfQuery : IRequest <GetBookingPdfResponse>
    {
        public int Id { get; set; }
    }
}
