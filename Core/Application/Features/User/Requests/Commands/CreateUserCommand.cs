using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public CreateUserDto CreateUserDto { get; set; } = null!;
    }
}
