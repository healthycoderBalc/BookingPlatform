using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.CreateCity
{
    public class CreateCityValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityValidator()
        {
            RuleFor(c => c.CreateCity.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.CreateCity.ThumbnailUrl)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.CreateCity.Country)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.CreateCity.PostalCode)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");
        }
    }
}
