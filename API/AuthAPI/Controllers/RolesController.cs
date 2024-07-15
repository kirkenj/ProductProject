﻿using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Roles")]
        public async Task<IEnumerable<RoleDto>> GetRolesList() => await _mediator.Send(new GetRoleListRequest());


        [HttpGet("Roles/{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var result = await _mediator.Send(new GetRoleDtoRequest() { Id = id });
            return result == null ? NotFound() : result;
        }
    }
}
