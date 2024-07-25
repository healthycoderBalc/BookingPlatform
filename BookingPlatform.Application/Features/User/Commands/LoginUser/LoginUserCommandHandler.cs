using AutoMapper;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingPlatform.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(ILogger<LoginUserCommandHandler> logger, SignInManager<Domain.Entities.User> signInManager, UserManager<Domain.Entities.User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginUserResponse = new LoginUserResponse();
            var validator = new LoginUserValidator(_userManager);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    loginUserResponse.Success = false;
                    loginUserResponse.ValidationErrors =
                        validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {;
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return loginUserResponse;
                }
                else if (loginUserResponse.Success)
                {
                    var user = await _userManager.FindByEmailAsync(request.LoginUser.Email);
                    string token = string.Empty;
                    if (user == null)
                    {
                        loginUserResponse.Success = false;
                        _logger.LogError($"Login Failed. No user registered with provided Email");
                        return loginUserResponse;
                    }
                    else
                    {
                        var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginUser.Password, false);
                        if (!result.Succeeded)
                        {
                            loginUserResponse.Success = false;
                            loginUserResponse.ValidationErrors = ["Login Failed"];
                            _logger.LogError($"Login Failed");
                            return loginUserResponse;
                        }
                        var roles = await _userManager.GetRolesAsync(user);
                        token = _jwtTokenGenerator.GenerateToken(user, roles);
                    }
                    loginUserResponse.Token = token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                loginUserResponse.Success = false;
                loginUserResponse.Message = ex.Message;
            }
            return loginUserResponse;

        }
    }
}
