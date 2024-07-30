using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch
{
    public class GetHotelsBySearchValidator : AbstractValidator<GetHotelsBySearchQuery>
    {
        public GetHotelsBySearchValidator()
        {
            RuleFor(h => h.CheckIn)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .When(h => h.CheckOut.HasValue)
               .WithMessage("CheckIn date must be provided if CheckOut date is specified.");

            RuleFor(h => h.CheckOut)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .When(h => h.CheckIn.HasValue)
                .WithMessage("CheckOut date must be provided if CheckIn date is specified.");

            RuleFor(h => h)
                .Must(h => !h.CheckIn.HasValue || !h.CheckOut.HasValue || h.CheckIn <= h.CheckOut)
                .WithMessage("CheckIn date must be before or equal to CheckOut date.");

            RuleFor(query => query.Adults)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Number of adults must be at least 1.");

            RuleFor(query => query.Children)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of children cannot be negative.");
        }
    }
}
