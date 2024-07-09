using Application.DTOs.Role;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleDtoRequest : IRequest<RoleDto>
    {
        public int Id { get; set; }
    }
}
