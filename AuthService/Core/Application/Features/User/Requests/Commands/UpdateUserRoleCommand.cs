using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserRoleCommand : IRequest<Response<string>>
    {
        public UpdateUserRoleDTO UpdateUserRoleDTO { get; set; } = null!;
    }
}
