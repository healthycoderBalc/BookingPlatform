using BookingPlatform.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.AssignRoleToUser
{
    public class AssignRoleToUserResponse : BaseResponse
    {
        public Unit Value { get; set; }
    }
}
