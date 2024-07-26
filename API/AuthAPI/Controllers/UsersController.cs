using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using AuthAPI.Extensions;
using Infrastructure.TockenTractker;
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
        private readonly TokenTracker<Guid> _tokenTracker;

        public UsersController(IMediator mediator, TokenTracker<Guid> tokenTracker)
        {
            _mediator = mediator;
            _tokenTracker = tokenTracker;
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

            if (result.Success)
            {
                _tokenTracker.InvalidateUser(request.Id, DateTime.UtcNow);
            }

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

            if (result.Success)
            {
                _tokenTracker.InvalidateUser(request.UserId, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("UserTag")]
        public async Task<ActionResult<string>> UpdateLogin(UpdateUserLoginDto request)
        {
            var result = await _mediator.Send(new UpdateUserLoginComand
            {
                UpdateUserLoginDto = request
            });

            if (result.Success)
            {
                _tokenTracker.InvalidateUser(request.Id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
