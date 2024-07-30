using BookingPlatform.Application.Features.Booking.Commands.AddToCart;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Payment.Commands.CreatePayment;
using BookingPlatform.Application.Features.Payment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponse>> Create(CreatePaymentDto payment)
        {
            var result = await Mediator.Send(new CreatePaymentCommand() { CreatePayment = payment });
            return result;
        }
    }
}
