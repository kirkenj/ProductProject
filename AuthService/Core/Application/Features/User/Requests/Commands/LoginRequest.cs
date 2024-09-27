using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class LoginRequest : IRequest<Response<UserDto>>
    {
        public LoginDto LoginDto { get; set; } = null!;
    }
}
