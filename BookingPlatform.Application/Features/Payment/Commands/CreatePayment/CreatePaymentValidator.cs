using FluentValidation;

namespace BookingPlatform.Application.Features.Payment.Commands.CreatePayment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator()
        {
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
        }
    }
}
