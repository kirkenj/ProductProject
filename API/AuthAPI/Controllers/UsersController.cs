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
        public async Task<IEnumerable<UserListDto>> Get() => await _mediator.Send(new GetUserListRequest());


        // GET api/<AuthController2>/5
        [HttpGet("Users/{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result == null ? NotFound() : result;
        }


        [HttpPut("Users")]
        [Authorize()]
        public async Task UpdatePassword(UpdateUserPasswordDto request)
        {
            await _mediator.Send(new UpdateUserPasswordComand
            {
                UpdateUserPasswordDto = new UpdateUserPasswordDto
                {
                    Id = User.GetUserId(),
                    OldPassword = request.OldPassword,
                    NewPassword = request.NewPassword
                }
            });
        }

        [HttpPut("Email")]
        public async Task UpdateEmail(UpdateUserEmailDto request)
        {
            await _mediator.Send(new UpdateUserEmailComand
            {
                UpdateUserEmailDto = new UpdateUserEmailDto
                {
                    Email = request.Email,
                    Id = request.Id
                }
            });
        }

        [HttpPost("SendEmailConfirmationToken")]
        public async Task<ActionResult<string>> SendEmailConfirmationToken(Guid userId)
        {
            return await _mediator.Send(new SendEmailConfirmationTokenQuery
            {
                SendEmailConfirmationTokenDto = new SendEmailConfirmationTokenDto
                {
                    UserID = userId
                }
            });
        }

        [HttpPost("ConfirmEmail")]
        public async Task<string> ConfirmEmail(Guid userId, string code)
        {
            return await _mediator.Send(new ConfirmEmailComand
            {
                ConfirmEmailDto = new ConfirmEmailDto
                {
                    Key = code,
                    UserId = userId
                }
            });
        }
    }
}
