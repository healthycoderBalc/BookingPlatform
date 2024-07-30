using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Commands.LoginUser
{
    public class LoginUserResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}
