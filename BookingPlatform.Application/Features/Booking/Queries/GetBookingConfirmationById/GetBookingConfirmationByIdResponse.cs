using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById
{
    public class GetBookingConfirmationByIdResponse : BaseResponse
    {
        public BookingConfirmationDto booking { get; set; }
    }
}
