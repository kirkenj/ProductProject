using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserPasswordComand : IRequest
    {
        public UpdateUserPasswordDto UpdateUserPasswordDto { get; set; } = null!;
    }
}
