using BookingPlatform.Application.Features.User.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserResponse>
    {
        public UserForLoginDto LoginUser { get; set; }
    }
}
