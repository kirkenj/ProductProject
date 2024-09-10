using Application.DTOs.Role;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleListRequest : IRequest<Response<List<RoleDto>>>
    {

    }
}
