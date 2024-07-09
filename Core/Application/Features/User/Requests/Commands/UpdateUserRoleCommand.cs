using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserRoleCommand : IRequest
    {
        public UpdateUserRoleDTO UpdateUserRoleDTO { get; set; } = null!;
    }
}
