using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class ConfirmEmailChangeComand : IRequest<Response<string>>
    {
        public ConfirmEmailChangeDto ConfirmEmailChangeDto { get; set; } = null!;
    }
}

