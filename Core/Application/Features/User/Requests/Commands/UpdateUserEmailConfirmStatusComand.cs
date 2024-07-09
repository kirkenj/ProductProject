using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateUserEmailConfirmStatusComand : IRequest
    {
        public UpdateUserEmailConfirmStatusDTO UpdateUserEmailConfirmStatusDTO { get; set; } = null!;
    }
}

