using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Role.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly RoleManager<Domain.Entities.Role> _roleManager;
        private readonly IMapper _mapper;

        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(RoleManager<Domain.Entities.Role> roleManager, IMapper mapper, ILogger<CreateRoleCommandHandler> logger)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var createRoleResponse = new CreateRoleResponse();
            var validator = new CreateRoleValidator();
            var roleExists = await _roleManager.RoleExistsAsync(request.CreateRole.Name);
            if (!roleExists)
            {
                var roleEntity = _mapper.Map<Domain.Entities.Role>(request.CreateRole);
                var result = await _roleManager.CreateAsync(roleEntity);
                if (!result.Succeeded)
                {
                    _logger.LogError($"Error while creating the role");
                }
                createRoleResponse.Value = Unit.Value;
            }
            return createRoleResponse;
        }
    }
}
