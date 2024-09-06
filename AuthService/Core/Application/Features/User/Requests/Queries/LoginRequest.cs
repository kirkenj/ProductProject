using Application.DTOs.User;
using Application.Models.Response;
using Application.Models.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class LoginRequest : IRequest<Response<LoginResult>>
    {
        public LoginDto LoginDto { get; set; } = null!;
    }
}
