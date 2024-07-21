using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class LoginDto : IRequest<Response<string?>>
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
