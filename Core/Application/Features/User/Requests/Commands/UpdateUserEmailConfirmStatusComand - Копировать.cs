using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserEmailComand : IRequest
    {
        public UpdateUserEmailDto UpdateUserEmailDto { get; set; } = null!;
    }
}

