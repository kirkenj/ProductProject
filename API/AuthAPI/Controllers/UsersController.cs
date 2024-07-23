using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using AuthAPI.Extensions;
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

        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> Get()
        {
            var result = await _mediator.Send(new GetUserListRequest());
            return result.GetActionResult();
        }

        [HttpGet("Users/{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result.GetActionResult();
        }


        [HttpPut]
        public async Task<ActionResult<string>> UpdateUser(UpdateNotSensetiveInfoDto request)
        {
            var result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserAddressDto = request
            });

            return result.GetActionResult();
        }

        [HttpPut("Password")]
        public async Task<ActionResult<string>> UpdatePassword(UpdateUserPasswordDto request)
        {
            var result = await _mediator.Send(new UpdateUserPasswordComand
            {
                UpdateUserPasswordDto = request
            });

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] SendTokenToUpdateUserEmailDto request)
        {
            var result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = request
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate(ConfirmEmailChangeDto request)
        {
            var result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = request
            });

            return result.GetActionResult();
        }
    }
}
