using AutoMapper;
using BookingPlatform.Application.Features.User.Dtos;
using BookingPlatform.Application.Features.User.Queries.GetUser;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Queries.GetUser
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByEmailQueryHandler> _logger;

        public GetUserByEmailQueryHandler(IUserRepository repository, IMapper mapper, ILogger<GetUserByEmailQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetUserByEmailResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var getUserResponse = new GetUserByEmailResponse();
            var validator = new GetUserByEmailValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    getUserResponse.Success = false;
                    getUserResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        getUserResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (getUserResponse.Success)
                {
                    var result = await _repository.GetUserByEmailAsync(request.Email);
                    getUserResponse.AuthUser = _mapper.Map<UserDto>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getUserResponse.Success = false;
                getUserResponse.Message = ex.Message;
            }

            return getUserResponse;
        }
    }
}
