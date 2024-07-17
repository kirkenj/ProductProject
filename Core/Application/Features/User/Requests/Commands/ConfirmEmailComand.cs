using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class ConfirmEmailComand : IRequest<Response<string>>
    {
        public ConfirmEmailDto ConfirmEmailDto { get; set; } = null!;
    }
}
