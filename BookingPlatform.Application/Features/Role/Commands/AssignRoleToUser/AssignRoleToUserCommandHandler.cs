using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Role.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, AssignRoleToUserResponse>
    {
        private readonly ILogger _logger;
        private readonly UserManager<Domain.Entities.User> _userManager;

        public AssignRoleToUserCommandHandler(ILogger<AssignRoleToUserCommandHandler> logger, UserManager<Domain.Entities.User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<AssignRoleToUserResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var assignRoleToUserResponse = new AssignRoleToUserResponse();
            var validator = new AssignRoleToUserValidator(_userManager);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    assignRoleToUserResponse.Success = false;
                    assignRoleToUserResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        assignRoleToUserResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (assignRoleToUserResponse.Success)
                {
                    var user = await _userManager.FindByIdAsync(request.AssignRole.UserId);
                    if (user != null)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, request.AssignRole.RoleName);
                        if (!roleResult.Succeeded)
                        {
                            _logger.LogError($"Role Assignment failed");
                        }
                    }
                    assignRoleToUserResponse.Value = Unit.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                assignRoleToUserResponse.Success = false;
                assignRoleToUserResponse.Message = ex.Message;
            }

            return assignRoleToUserResponse;
        }
    }
}
