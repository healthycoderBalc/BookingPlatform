using BookingPlatform.Application.Features.User.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.User.Queries.GetUser
{
    public class GetUserByEmailResponse : BaseResponse
    {
        public UserDto AuthUser { get; set; }
    }
}
