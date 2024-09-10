using Application.DTOs.Role;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetRoleDtoRequest : IRequest<Response<RoleDto>>
    {
        public int Id { get; set; }
    }
}
