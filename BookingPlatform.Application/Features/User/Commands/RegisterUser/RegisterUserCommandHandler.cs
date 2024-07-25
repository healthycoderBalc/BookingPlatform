using AutoMapper;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserCommandHandler(IMapper mapper, ILogger<RegisterUserCommandHandler> logger, UserManager<Domain.Entities.User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var registerUserResponse = new RegisterUserResponse();
            var validator = new RegisterUserValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    registerUserResponse.Success = false;
                    registerUserResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return registerUserResponse;
                }
                else if (registerUserResponse.Success)
                {
                    var userEntity = _mapper.Map<Domain.Entities.User>(request.RegisterUser);
                    var result = await _userManager.CreateAsync(userEntity, request.RegisterUser.Password);
                    if (!result.Succeeded)
                    {
                        registerUserResponse.Success = false;
                        registerUserResponse.ValidationErrors = result.Errors.Select(e => e.Description).ToList();
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError($"Registration failed due to error- {error.Description}");
                        }
                        return registerUserResponse;
                    }
                    await _userManager.AddToRoleAsync(userEntity, "Customer");
                    var roles = await _userManager.GetRolesAsync(userEntity);
                    var token = _jwtTokenGenerator.GenerateToken(userEntity, roles);
                    registerUserResponse.Token = token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                registerUserResponse.Success = false;
                registerUserResponse.Message = ex.Message;
            }

            return registerUserResponse;
        }
    }
}
