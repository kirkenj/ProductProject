﻿using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Features.User.Requests.Queries;
using Application.Models.User;
using AuthAPI.ActionFIlters;
using AuthAPI.Contracts;
using AuthAPI.Models.Requests;
using Constants;
using CustomResponse;
using Extensions.ClaimsPrincipalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


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

        [HttpPut("{id}")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> Put(Guid id, UpdateUserModel updateUserModel)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new UpdateNotSensitiveUserInfoComand
            {
                UpdateUserInfoDto = new UpdateUserInfoDto
                {
                    Id = id,
                    Address = updateUserModel.Address,
                    Name = updateUserModel.Name
                }
            });

            return result.GetActionResult();
        }

        [HttpPut("{id}/Email")]
        [Authorize(ApiConstants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateEmail(Guid id, [FromBody][EmailAddress] string newEmail)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new SendTokenToUpdateUserEmailRequest
            {
                UpdateUserEmailDto = new UpdateUserEmailDto { Id = id, Email = newEmail }
            });

            return result.GetActionResult();
        }

        [HttpPost("{id}/Email/Confirm")]
        [Authorize]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> ConfirmEmailUpdate(Guid id, [FromBody][EmailAddress] string confirmToken)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new ConfirmEmailChangeDto { Id = id, Token = confirmToken }
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("{id}/Login")]
        [Produces("text/plain")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateLogin(Guid id, string newLogin)
        {
            if (!User.IsInRole(ApiConstants.ADMIN_ROLE_NAME) && id != User.GetUserId())
            {
                return Forbid();
            }

            Response<string> result = await _mediator.Send(new UpdateUserLoginComand
            {
                UpdateUserLoginDto = new UpdateUserLoginDto { Id = id, NewLogin = newLogin }
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }

        [HttpPut("{id}/Role")]
        [Authorize(ApiConstants.ADMIN_POLICY_NAME)]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> UpdateRole(Guid id, int roleId)
        {
            Response<string> result = await _mediator.Send(new UpdateUserRoleCommand
            {
                UpdateUserRoleDTO = new UpdateUserRoleDTO { Id = id, RoleID = roleId }
            });

            if (result.Success)
            {
                await _tokenTracker.InvalidateUser(id, DateTime.UtcNow);
            }

            return result.GetActionResult();
        }
    }
}
