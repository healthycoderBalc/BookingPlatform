using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Commands.LoginUser
{
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;

        public LoginUserValidator(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;

            RuleFor(c => c.LoginUser.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value")
                .EmailAddress()
                .WithMessage("{ PropertyName} should be a valid Email");


            RuleFor(c => c.LoginUser.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.LoginUser.Email)
                  .MustAsync(emailExist)
                  .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> emailExist(string email, CancellationToken cancellation)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user != null;
        }
    }
}
