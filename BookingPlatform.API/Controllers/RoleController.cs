using BookingPlatform.Application.Features.Role.Commands.AssignRoleToUser;
using BookingPlatform.Application.Features.Role.Commands.CreateRole;
using BookingPlatform.Application.Features.Role.Dtos;
using BookingPlatform.Application.Features.Role.Queries.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ApiControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<GetRolesResponse>> GetAll()
        {
            var response = await Mediator.Send(new GetRolesQuery());
            return response;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateRoleResponse>> Create(RoleDto role)
        {
            var result = await Mediator.Send(new CreateRoleCommand() { CreateRole = role });
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assignrole")]
        public async Task<ActionResult<AssignRoleToUserResponse>> AssignRole(UserRoleDto userRole)
        {
            var result = await Mediator.Send(new AssignRoleToUserCommand()
            {
                AssignRole = userRole
            });
            return result;
        }
    }
}
