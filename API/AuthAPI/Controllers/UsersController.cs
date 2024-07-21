using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using AuthAPI.Extensions;
using AuthAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<UserListDto>>> Get()
        {
            var result = await _mediator.Send(new GetUserListRequest());
            return result.GetActionResult();
        }

        // GET api/<AuthController2>/5
        [HttpGet("Users/{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result.GetActionResult();
        }


        [HttpPut("Users")]
        [Authorize()]
        public async Task<ActionResult<string>> UpdatePassword(UpdateUserPasswordDto request)
        {
            var result = await _mediator.Send(new UpdateUserPasswordComand
            {
                UpdateUserPasswordDto = new UpdateUserPasswordDto
                {
                    Id = User.GetUserId(),
                    NewPassword = request.NewPassword
                }
            });
            return result.GetActionResult();
        }

        [HttpPut("Email")]
        public async Task<ActionResult<string>> UpdateEmail(UpdateUserEmailDto request)
        {
            var result = await _mediator.Send(new UpdateUserEmailComand
            {
                UpdateUserEmailDto = new UpdateUserEmailDto
                {
                    Email = request.Email,
                    Id = request.Id
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("SendEmailConfirmationToken")]
        public async Task<ActionResult<string>> SendEmailConfirmationToken(Guid userId)
        {
            var result = await _mediator.Send(new SendEmailConfirmationTokenQuery
            {
                SendEmailConfirmationTokenDto = new SendEmailConfirmationTokenDto
                {
                    UserID = userId
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail(Guid userId, string code)
        {
            var result = await _mediator.Send(new ConfirmEmailComand
            {
                ConfirmEmailDto = new ConfirmEmailDto
                {
                    Key = code,
                    UserId = userId
                }
            });

            return result.GetActionResult();
        }
    }
}
