using Application.DTOs.Role;
using CustomResponse;
using MediatR;

namespace Application.Features.Role.Requests.Queries
{
    public class GetRoleListRequest : IRequest<Response<List<RoleDto>>>
    {

    }
}
