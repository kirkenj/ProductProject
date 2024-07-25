using Application.DTOs.User;
using Application.Features.Tokens.Requests.Commands;
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
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("TokenClaims")]
        public IEnumerable<KeyValuePair<string, string>> GetTokenClaims() => User.Claims.Select(o => new KeyValuePair<string, string>(o.Type ?? "Unknown", o.Value ?? "Unknown"));

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAccountDetails()
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = User.GetUserId() });
            return result.GetActionResult();
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserModel updateUserModel)
        {
            var result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand 
            { 
                UpdateUserAddressDto = new UpdateNotSensetiveInfoDto 
                { 
                    Id = User.GetUserId(), 
                    Address = updateUserModel.Address,
                    Name = updateUserModel.Name
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("Password")]
        public async Task<ActionResult<string>> UpdatePassword(AuthorizedUserUpdatePassword request)
        {
            var result = await _mediator.Send(new UpdateUserPasswordComand { UpdateUserPasswordDto = new UpdateUserPasswordDto
            { 
                Id = User.GetUserId(),
                NewPassword = request.NewPassword
            }
            });

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody]string newEmail)
        {
            var result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = new SendTokenToUpdateUserEmailDto
                {
                    Email = newEmail,
                    Id = User.GetUserId()
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate([FromBody]string confirmationToken)
        {
            var result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new ConfirmEmailChangeDto
                {
                    UserId = User.GetUserId(),
                    Token = confirmationToken
                }
            });

            if (result.Success)
            {
                //await _mediator.Send(new InvalidateUserTokensCommand 
                //{ 
                //    InvalidateUserTokensDto = new () { Token = Request.Headers.Authorization.ToString().Split(' ')[1] } 
                //});
            }

            return result.GetActionResult();
        }
    }
}
