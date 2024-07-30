using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<CreatePaymentResponse>
    {
        public CreatePaymentDto CreatePayment { get; set; }
    }
}
