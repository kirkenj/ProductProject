using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class SendEmailConfirmationTokenQuery : IRequest<string>
    {
        public SendEmailConfirmationTokenDto SendEmailConfirmationTokenDto { get; set; } = null!;
    }
}
