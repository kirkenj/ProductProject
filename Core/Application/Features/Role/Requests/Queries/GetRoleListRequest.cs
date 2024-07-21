using Application.DTOs.Role;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleListRequest : IRequest<Response<List<RoleDto>>>
    {

    }
}
