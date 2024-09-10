using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using Application.Models.User;
using AuthAPI.FIlters;
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomResponse;


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

        [HttpGet("list")]
        [GetUserActionFilter]
        public async Task<ActionResult<IEnumerable<UserListDto>>> Get([FromQuery] UserFilter filter, int? page, int? pageSize)
        {
            if (!User.IsInRole(Constants.Constants.ADMIN_ROLE_NAME))
            {
                filter.RoleIds = null;
            }

            Response<List<UserListDto>> result = await _mediator.Send(new GetUserListRequest() { UserFilter = filter, Page = page, PageSize = pageSize });
            return result.GetActionResult();
        }

        [HttpGet("{id}")]
        [GetUserActionFilter]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            Response<UserDto> result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result.GetActionResult();
        }

        [HttpPut]
        [Authorize(Constants.Constants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateUser(UpdateNotSensetiveInfoDto request)
        {
            Response<string> result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateNotSensetiveInfoDto = request
            });

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        [Authorize(Constants.Constants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] SendTokenToUpdateUserEmailDto request)
        {
            Response<string> result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = request
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        [Authorize(Constants.Constants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate(ConfirmEmailChangeDto request)
        {
            Response<string> result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = request
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(request.UserId, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("UserTag")]
        [Authorize(Constants.Constants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateLogin(UpdateUserLoginDto request)
        {
            Response<string> result = await _mediator.Send(new UpdateUserLoginComand
            {
                UpdateUserLoginDto = request
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(request.Id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
