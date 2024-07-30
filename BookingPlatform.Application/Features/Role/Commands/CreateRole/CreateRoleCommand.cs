using BookingPlatform.Application.Features.Role.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<CreateRoleResponse>
    {
        public RoleDto CreateRole { get; set; }
    }
}
