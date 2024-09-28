using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class SendTokenToUpdateUserEmailRequest : IRequest<Response<string>>
    {
        public UpdateUserEmailDto UpdateUserEmailDto { get; set; } = null!;
    }
}
