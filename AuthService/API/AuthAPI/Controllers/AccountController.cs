using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using AuthAPI.Extensions;
using AuthAPI.Models;
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly TokenTracker<Guid> _tokenTracker;

        public AccountController(IMediator mediator, TokenTracker<Guid> tokenTracker)
        {
            _tokenTracker = tokenTracker;
            _mediator = mediator;
        }

        [HttpGet("TokenClaims")]
        public IEnumerable<KeyValuePair<string, string>> GetTokenClaims() => User.Claims.Select(o => new KeyValuePair<string, string>(o.Type ?? "Unknown", o.Value ?? "Unknown"));

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAccountDetails()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new GetUserDtoRequest() { Id = userId.Value });
            return result.GetActionResult();
        }

        [HttpPut]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserModel updateUserModel)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserAddressDto = new UpdateNotSensetiveInfoDto
                {
                    Id = userId.Value,
                    Address = updateUserModel.Address,
                    Name = updateUserModel.Name
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("Password")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdatePassword(AuthorizedUserUpdatePassword request)
        {

            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateUserPasswordComand
            {
                UpdateUserPasswordDto = new UpdateUserPasswordDto
                {
                    Id = userId.Value,
                    NewPassword = request.NewPassword
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("UserTag")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateLogin(string newLogin)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateUserLoginComand
            {
                UpdateUserLoginDto = new()
                {
                    Id = userId.Value,
                    NewLogin = newLogin
                }
            });

            if (result.Success)
            {
                result.Result += "Relogin needed.";
                _tokenTracker.InvalidateUser(userId.Value, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] string newEmail)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = new SendTokenToUpdateUserEmailDto
                {
                    Email = newEmail,
                    Id = userId.Value
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate([FromBody] string confirmationToken)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new ConfirmEmailChangeDto
                {
                    UserId = userId.Value,
                    Token = confirmationToken
                }
            });

            if (result.Success)
            {
                result.Result += "Relogin needed.";
                _tokenTracker.InvalidateUser(userId.Value, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
