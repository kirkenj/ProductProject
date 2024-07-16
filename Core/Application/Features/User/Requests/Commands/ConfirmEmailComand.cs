using Application.DTOs.User;
using Application.Models;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class ConfirmEmailComand : IRequest<Response>
    {
        public ConfirmEmailDto ConfirmEmailDto { get; set; } = null!;
    }
}
