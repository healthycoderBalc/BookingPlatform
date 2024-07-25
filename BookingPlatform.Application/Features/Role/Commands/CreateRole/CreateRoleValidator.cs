using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(a => a.CreateRole.Name)
               .NotEmpty()
               .NotNull()
               .WithMessage("{PropertyName} should have a value");
        }
    }
}
