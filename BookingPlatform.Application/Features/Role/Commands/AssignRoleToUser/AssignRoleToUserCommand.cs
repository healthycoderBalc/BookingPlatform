using BookingPlatform.Application.Features.Role.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommand : IRequest<AssignRoleToUserResponse>
    {
        public UserRoleDto AssignRole { get; set; }
    }
}
