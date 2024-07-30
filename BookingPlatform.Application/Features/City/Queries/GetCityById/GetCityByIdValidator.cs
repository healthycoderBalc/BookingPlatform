using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCityById
{
    public class GetCityByIdValidator : AbstractValidator<GetCityByIdQuery>
    {
        public GetCityByIdValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");
        }
    }
}
