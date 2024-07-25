using BookingPlatform.Application.Features.Role.Dtos;
using BookingPlatform.Application.Response;

namespace BookingPlatform.Application.Features.Role.Queries.GetRoles
{
    public class GetRolesResponse : BaseResponse
    {
        public ICollection<RoleDto> Roles { get; set; }
    }
}
