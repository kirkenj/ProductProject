using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class CreateUserCommand : IRequest<Response<Guid>>
    {
        public CreateUserDto CreateUserDto { get; set; } = null!;
    }
}
