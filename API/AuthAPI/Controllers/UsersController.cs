using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
