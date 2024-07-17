using Application.DTOs.Role;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthAPI.Extensions;
using Application.Features.User.Requests.Commands;
using Application.DTOs.User.Interfaces;
using AuthAPI.Models;
using Application.Features.User.Handlers.Queries;

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
        public async Task<UserDto> GetAccountDetails()
        {
            return await _mediator.Send(new GetUserDtoRequest() { Id = User.GetUserId() });
        }

        [HttpPut("Address")]
        public async Task UpdateUser(string newAddress)
        {
            await _mediator.Send(new UpdateNotSensitiveUserInfoComand 
            { 
                UpdateUserAddressDto = new UpdateUserAddressDto 
                { 
                    Id = User.GetUserId(), 
                    Address = newAddress
                }
            });
        }

        [HttpPut("Password")]
        public async Task UpdatePassword(AuthorizedUserUpdatePassword request)
        {
            await _mediator.Send(new UpdateUserPasswordComand { UpdateUserPasswordDto = new UpdateUserPasswordDto
            { 
                Id = User.GetUserId(),
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            }
            });
        }

        [HttpPut("Email")]
        public async Task UpdateEmail(string newEmail)
        {
            await _mediator.Send(new UpdateUserEmailComand
            {
                UpdateUserEmailDto = new UpdateUserEmailDto
                {
                    Email = newEmail,
                    Id = User.GetUserId()
                }
            });
        }

        [HttpPost("SendEmailConfirmationToken")]
        public async Task<string> SendEmailConfirmationToken()
        {
            return await _mediator.Send(new SendEmailConfirmationTokenQuery 
            { 
                SendEmailConfirmationTokenDto = new SendEmailConfirmationTokenDto 
                { 
                    UserID = User.GetUserId() 
                } 
            });
        }

        [HttpPost("ConfirmEmail")]
        public async Task<string> ConfirmEmail(string code)
        {
            return await _mediator.Send(new ConfirmEmailComand
            {
                ConfirmEmailDto = new ConfirmEmailDto
                {
                    Key = code,
                    UserId = User.GetUserId()
                }
            });
        }
    }
}
