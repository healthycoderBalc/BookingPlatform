using BookingPlatform.Application.Features.Booking.Commands.AddToCart;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf;
using BookingPlatform.Application.Features.City.Commands.CreateCity;
using BookingPlatform.Application.Features.City.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ApiControllerBase
    {
        [Authorize]
        [HttpPost("add-to-cart")]
        public async Task<ActionResult<AddToCartResponse>> Create(AddToCartBookingDto booking)
        {
            var result = await Mediator.Send(new AddToCartCommand() { AddToCartBooking = booking });
            return result;
        }

        [Authorize]
        [HttpGet("{id}/confirmation/")]
        public async Task<ActionResult<GetBookingConfirmationByIdResponse>> GetConfirmationById(int id)
        {
            var result = await Mediator.Send(new GetBookingConfirmationByIdQuery() { Id = id });
            return result;
        }

        [Authorize]
        [HttpGet("{id}/confirmation/download/")]
        public async Task<ActionResult<GetBookingPdfResponse>> DownloadConfirmation(int id)
        {
            var result = await Mediator.Send(new GetBookingPdfQuery() { Id = id});
         
            if (result.Success)
            {
                return File(result.PdfBytes, "application/pdf", "confirmation.pdf");
            }

            return NotFound(result.Message);
        }
    }
}
