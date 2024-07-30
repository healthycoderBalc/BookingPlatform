using BookingPlatform.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.AssignRoleToUser
{
    public class AssignRoleToUserValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;


        public AssignRoleToUserValidator(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;

            RuleFor(a => a.AssignRole.UserId)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(a => a.AssignRole.RoleName)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.AssignRole.UserId)
             .MustAsync(userExist)
             .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> userExist(string userId, CancellationToken cancellation)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            return user != null;
        }
    }
}
