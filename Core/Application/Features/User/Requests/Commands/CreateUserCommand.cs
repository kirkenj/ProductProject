using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class CreateUserCommand : IRequest<Response<Guid>>
    {
        public CreateUserDto CreateUserDto { get; set; } = null!;
    }
}
