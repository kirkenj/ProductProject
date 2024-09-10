using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class ForgotPasswordComand : IRequest<Response<string>>
    {
        public ForgotPasswordDto ForgotPasswordDto { get; set; } = null!;
    }
}
