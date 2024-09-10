using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class SendTokenToUpdateUserEmailRequest : IRequest<Response<string>>
    {
        public SendTokenToUpdateUserEmailDto UpdateUserEmailDto { get; set; } = null!;
    }
}
