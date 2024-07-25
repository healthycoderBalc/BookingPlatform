using BookingPlatform.Application.Features.User.Commands.LoginUser;
using BookingPlatform.Application.Features.User.Commands.RegisterUser;
using BookingPlatform.Application.Features.User.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserResponse>> Register(UserForRegistrationDto user)
        {
            var response = await Mediator.Send(new RegisterUserCommand() { RegisterUser = user});
            return response;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> Login(UserForLoginDto user)
        {
            var response = await Mediator.Send(new LoginUserCommand() { LoginUser = user});
            return response;
        }
    }
}
