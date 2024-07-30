using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(c => c.RegisterUser.FirstName)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.RegisterUser.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.RegisterUser.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value")
                .EmailAddress()
                .WithMessage("{PropertyName} should be an email");
            RuleFor(c => c.RegisterUser.DateOfBirth)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");
            RuleFor(c => c.RegisterUser.Password)
                  .NotEmpty()
                  .NotNull()
                  .WithMessage("{PropertyName} should have a value")
                  .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long")
                  .Matches("[A-Z]").WithMessage("{PropertyName} must contain at least one uppercase letter")
                  .Matches("[a-z]").WithMessage("{PropertyName} must contain at least one lowercase letter")
                  .Matches("[0-9]").WithMessage("{PropertyName} must contain at least one number")
                  .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must contain at least one non-alphanumeric character");
        }
    }
}
