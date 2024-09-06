using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using Application.Models.User;
using AuthAPI.Extensions;
using AuthAPI.FIlters;
using Infrastructure.TockenTractker;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;

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
        public async Task<ActionResult<IEnumerable<UserListDto>>> Get([FromQuery]UserFilter filter, int? page, int? pageSize)
        {
            if (!User.IsInRole(Constants.Constants.ADMIN_ROLE_NAME))
            {
                filter.RoleIds = null;
            }

            var result = await _mediator.Send(new GetUserPagedFilteredListRequest() { UserFilter = filter, Page = page, PageSize = pageSize});
            return result.GetActionResult();
        }

        [HttpGet("{id}")]
        [GetUserActionFilter]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserDtoRequest() { Id = id });
            return result.GetActionResult();
        }

        [HttpPut]
        [Authorize(AuthAPI.Constants.Constants.ADMIN_POLICY_NAME)]
        public async Task<ActionResult<string>> UpdateUser(UpdateNotSensetiveInfoDto request)
        {
            var result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserAddressDto = request
            });

            return result.GetActionResult();
        }

        [HttpPut("Email")]
        [Authorize(AuthAPI.Constants.Constants.ADMIN_POLICY_NAME)]
        public async Task<ActionResult<string>> UpdateEmail([FromBody] SendTokenToUpdateUserEmailDto request)
        {
            var result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = request
            });

            return result.GetActionResult();
        }

        [HttpPost("Email")]
        [Authorize(AuthAPI.Constants.Constants.ADMIN_POLICY_NAME)]
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
        [Authorize(AuthAPI.Constants.Constants.ADMIN_POLICY_NAME)]
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
