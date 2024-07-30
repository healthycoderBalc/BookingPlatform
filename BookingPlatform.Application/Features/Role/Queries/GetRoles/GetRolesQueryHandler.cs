using AutoMapper;
using BookingPlatform.Application.Features.Role.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Role.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, GetRolesResponse>
    {
        private readonly IRepository<Domain.Entities.Role> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRolesQueryHandler> _logger;

        public GetRolesQueryHandler(IRepository<Domain.Entities.Role> repository, IMapper mapper, ILogger<GetRolesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetRolesResponse> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var getRoleResponse = new GetRolesResponse();
            var validator = new GetRolesValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count >0)
                {
                    getRoleResponse.Success = false;
                    getRoleResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        getRoleResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (getRoleResponse.Success)
                {
                    var result = await _repository.GetAllAsync();
                    getRoleResponse.Roles = _mapper.Map<ICollection<RoleDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getRoleResponse.Success = false;
                getRoleResponse.Message = ex.Message;
            }

            return getRoleResponse;
        }
    }
}
