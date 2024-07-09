using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserPasswordComand : IRequest
    {
        public UpdateUserPasswordDTO UpdateUserPasswordDTO { get; set; } = null!;
    }
}
