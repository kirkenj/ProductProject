using Application.DTOs.Role;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Features.User.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        // POST api/<AuthController2>
        [HttpPost("Register")]
        public async Task<Guid> Post([FromBody] CreateUserDto createUserDto)
        {
            var result = await _mediator.Send(new CreateUserCommand() { CreateUserDto = createUserDto });
            return result;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto) 
        {
            var result = await _mediator.Send(loginDto);
            if (result == null)
            {
                return false.ToString();
            }

            return result;
        }
    }
}
