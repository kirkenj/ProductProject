using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using AuthAPI.Contracts;
using AuthAPI.Models.Requests;
using CustomResponse;
using Extensions.ClaimsPrincipalExtensions;
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
        private readonly ITokenTracker<Guid> _tokenTracker;

        public AccountController(IMediator mediator, ITokenTracker<Guid> tokenTracker)
        {
            _tokenTracker = tokenTracker;
            _mediator = mediator;
        }

        [HttpGet("TokenClaims")]
        public IEnumerable<KeyValuePair<string, string>> GetTokenClaims() => User.Claims.Select(o => new KeyValuePair<string, string>(o.Type ?? "Unknown", o.Value ?? "Unknown"));

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAccountDetails()
        {
            Response<UserDto> result = await _mediator.Send(new GetUserDtoRequest()
            {
                Id = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id")
            });
            return result.GetActionResult();
        }

        [HttpPut]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserModel updateUserModel)
        {
            var userId = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id");

            Response<string> result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserInfoDto = new UpdateUserInfoDto
                {
                    Id = userId,
                    Address = updateUserModel.Address,
                    Name = updateUserModel.Name
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("Password")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdatePassword([FromBody] string request)
        {

            Response<string> result = await _mediator.Send(new UpdateUserPasswordComand
            {
                UpdateUserPasswordDto = new UpdateUserPasswordDto
                {
                    Id = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id"),
                    Password = request
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("UserTag")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateLogin(string newLogin)
        {
            var userId = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id");

            Response<string> result = await _mediator.Send(new UpdateUserLoginComand
            {
                UpdateUserLoginDto = new()
                {
                    Id = userId,
                    NewLogin = newLogin
                }
            });

            if (result.Success)
            {
                result.Result += "Relogin needed.";
                await _tokenTracker.InvalidateUser(userId, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] string newEmail)
        {
            Response<string> result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = new UpdateUserEmailDto
                {
                    Email = newEmail,
                    Id = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id")
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate([FromBody] string confirmationToken)
        {
            var userId = User.GetUserId() ?? throw new ApplicationException("Couldn't get user's id");

            Response<string> result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new ConfirmEmailChangeDto
                {
                    Id = userId,
                    Token = confirmationToken
                }
            });

            if (result.Success)
            {
                result.Result += "Relogin needed.";
                await _tokenTracker.InvalidateUser(userId, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
