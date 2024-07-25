using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Queries.GetUser
{
    public class GetUserByEmailQuery : IRequest<GetUserByEmailResponse>
    {
        public string Email { get; set; }
    }
}
