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

        [HttpPut("Address")]
        public async Task<ActionResult<string>> UpdateUser(string newAddress)
        {
            var result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand 
            { 
                UpdateUserAddressDto = new UpdateUserAddressDto 
                { 
                    Id = User.GetUserId(), 
                    Address = newAddress
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
        public async Task<ActionResult<string>> UpdateEmail(string newEmail)
        {
            var result = await _mediator.Send(new UpdateUserEmailComand
            {
                UpdateUserEmailDto = new UpdateUserEmailDto
                {
                    Email = newEmail,
                    Id = User.GetUserId()
                }
            });

            return result.GetActionResult();
        }

        [HttpPost("SendEmailConfirmationToken")]
        public async Task<ActionResult<string>> SendEmailConfirmationToken()
        {
            var result = await _mediator.Send(new SendEmailConfirmationTokenQuery 
            { 
                SendEmailConfirmationTokenDto = new SendEmailConfirmationTokenDto 
                { 
                    UserID = User.GetUserId() 
                } 
            });

            return result.GetActionResult();
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail(string code)
        {
            var result = await _mediator.Send(new ConfirmEmailComand 
            { 
                ConfirmEmailDto = new ConfirmEmailDto 
                { 
                    Key = code, 
                    UserId = User.GetUserId() 
                } 
            });
            return result.GetActionResult();
        }


        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPassword(string email)
        {
            var result = await _mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = new ForgotPasswordDto
                {
                    Email = email
                }
            });
            return result.GetActionResult();
        }
    }
}
