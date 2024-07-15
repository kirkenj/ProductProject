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


        // GET: api/<AuthController2>
        [HttpGet("Roles")]
        public async Task<IEnumerable<RoleDto>> GetRolesList() => await _mediator.Send(new GetRoleListRequest());


        // GET api/<AuthController2>/5
        [HttpGet("Roles/{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var result = await _mediator.Send(new GetRoleDtoRequest() { Id = id });
            return result == null ? NotFound() : result;
        }

        // GET: api/<AuthController2>
        [HttpGet("Users")]
        public async Task<IEnumerable<UserListDto>> Get() => await _mediator.Send(new GetUserListRequest());


        // GET api/<AuthController2>/5
        [HttpGet("Users/{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result == null ? NotFound() : result;
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


        [Authorize()]
        [HttpGet("Account")]
        public string GetLogin()
        {
            return User.Identity.Name;
        }

        // PUT api/<AuthController2>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController2>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
