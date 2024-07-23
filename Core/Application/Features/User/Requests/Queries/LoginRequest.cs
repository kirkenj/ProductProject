using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class LoginRequest : IRequest<Response<string?>>
    {
        public LoginDto LoginDto { get; set; } = null!;
    }
}
