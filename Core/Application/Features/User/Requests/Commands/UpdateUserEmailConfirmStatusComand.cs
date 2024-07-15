using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserEmailConfirmStatusComand : IRequest
    {
        public UpdateUserEmailConfirmStatusDto UpdateUserEmailConfirmStatusDto { get; set; } = null!;
    }
}

