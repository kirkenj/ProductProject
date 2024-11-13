using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using Application.Models.User;
using AuthAPI.Contracts;
using AuthAPI.FIlters;
using Constants;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenTracker<Guid> _tokenTracker;

        public UsersController(IMediator mediator, ITokenTracker<Guid> tokenTracker)
        {
            _mediator = mediator;
            _tokenTracker = tokenTracker;
        }

        [HttpGet("list")]
        [GetUserActionFilter]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] UserFilter filter, int? page, int? pageSize)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME))
            {
                filter.RoleIds = null;
                filter.Address = null;
            }

            Response<List<UserDto>> result = await _mediator.Send(new GetUserListRequest() { UserFilter = filter, Page = page, PageSize = pageSize });
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
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserInfoDto request)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && request.Id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserInfoDto = request
            });

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        [Authorize(ApiConstants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] UpdateUserEmailDto request)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && request.Id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = request
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate(ConfirmEmailChangeDto request)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && request.Id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = request
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(request.Id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("UserTag")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateLogin(UpdateUserLoginDto request)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && request.Id != User.GetUserId())
            {
                return Forbid();
            }

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

        [HttpPut("Role")]
        [Authorize(ApiConstants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateRole(UpdateUserRoleDTO request)
        {
            Response<string> result = await _mediator.Send(new UpdateUserRoleCommand
            {
                UpdateUserRoleDTO = request
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(request.Id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
