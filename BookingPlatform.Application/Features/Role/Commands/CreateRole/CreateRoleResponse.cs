using BookingPlatform.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Role.Commands.CreateRole
{
    public class CreateRoleResponse : BaseResponse
    {
        public Unit Value { get; set; }
    }
}
