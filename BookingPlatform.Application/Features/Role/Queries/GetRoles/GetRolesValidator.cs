using FluentValidation;

namespace BookingPlatform.Application.Features.Role.Queries.GetRoles
{
    public class GetRolesValidator : AbstractValidator<GetRolesQuery>
    {
        public GetRolesValidator()
        {
        }
    }
}
