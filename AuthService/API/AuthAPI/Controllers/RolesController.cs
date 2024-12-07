using Application.DTOs.Role;
using Application.Features.Role.Requests.Queries;
using CustomResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRolesList()
        {
            Response<List<RoleDto>> result = await _mediator.Send(new GetRoleListRequest());
            return result.GetActionResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            Response<RoleDto> result = await _mediator.Send(new GetRoleDtoRequest() { Id = id });
            return result.GetActionResult();
        }
    }
}
