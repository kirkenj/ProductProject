using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserRoleCommand : IRequest<Response<string>>
    {
        public UpdateUserRoleDTO UpdateUserRoleDTO { get; set; } = null!;
    }
}
