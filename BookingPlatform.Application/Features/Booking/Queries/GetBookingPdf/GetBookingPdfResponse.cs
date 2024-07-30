using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf
{
    public class GetBookingPdfResponse : BaseResponse
    {
        public byte[] PdfBytes { get; set; }
    }
}
