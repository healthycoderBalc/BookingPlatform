using BookingPlatform.Application.Interfaces;
using FluentValidation;

namespace BookingPlatform.Application.Features.Payment.Commands.CreatePayment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        private readonly IBookingRepository _bookingRepository;

        public CreatePaymentValidator(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;

            RuleFor(c => c.CreatePayment.BookingId)
               .GreaterThan(0)
               .WithMessage("{PropertyName} greater than 0");

            RuleFor(a => a.CreatePayment.PaymentMethod)
                 .NotEmpty()
                 .NotNull()
                 .WithMessage("{PropertyName} should have a value");

            RuleFor(a => a.CreatePayment.PaymentStatus)
                 .NotEmpty()
                 .NotNull()
                 .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreatePayment.BookingId)
             .MustAsync(BookingExist)
             .WithMessage("Booking does not exist");
        }

        private async Task<bool> BookingExist(int bookingId, CancellationToken cancellation)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId).ConfigureAwait(false);
            return booking?.Id > 0;
        }
    }
}
