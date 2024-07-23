using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Amenity.Commands.CreateAmenity
{
    public class CreateAmenityValidator : AbstractValidator<CreateAmenityCommand>
    {
        public CreateAmenityValidator()
        {
            RuleFor(a => a.CreateAmenity.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");
        }
    }
}
