using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using Application.Models.User;
using CustomResponse; 
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly TokenTracker<Guid> _tokenTracker;

        public AuthController(IMediator mediator, TokenTracker<Guid> tokenTracker)
        {
            _mediator = mediator;
            _tokenTracker = tokenTracker;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register([FromBody] CreateUserDto createUserDto)
        {
            Response<Guid> result = await _mediator.Send(new CreateUserCommand() { CreateUserDto = createUserDto });
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return result.GetActionResult();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResult>> Login(LoginDto loginDto)
        {
            Response<LoginResult> result = await _mediator.Send(new LoginRequest { LoginDto = loginDto });

            if (result.Success)
            {
                await _tokenTracker.Track(new KeyValuePair<string, AssignedTokenInfo<Guid>>
                        (
                            result.Result?.Token ?? throw new ApplicationException(),
                            new()
                            {
                                DateTime = DateTime.UtcNow,
                                UserId = result.Result.UserId
                            }
                        ));

            }
            return result.GetActionResult();
        }

        [HttpPost("ForgotPassword")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ForgotPassword([FromBody][EmailAddress] string email)
        {
            Response<string> result = await _mediator.Send(new ForgotPasswordComand
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
