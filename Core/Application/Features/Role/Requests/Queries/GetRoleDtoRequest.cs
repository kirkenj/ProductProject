using Application.DTOs.Role;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleDtoRequest : IRequest<Response<RoleDto>>
    {
        public int Id { get; set; }
    }
}
