using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdValidator : AbstractValidator<GetHotelByIdQuery>
    {
        public GetHotelByIdValidator()
        {
            RuleFor(c => c.Id)
               .GreaterThan(0)
               .WithMessage("{PropertyName} greater than 0");
        }
    }
}
