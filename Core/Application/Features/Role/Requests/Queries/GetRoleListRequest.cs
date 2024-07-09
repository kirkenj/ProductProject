using Application.DTOs.Role;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleListRequest : IRequest<List<RoleDto>>
    {

    }
}
