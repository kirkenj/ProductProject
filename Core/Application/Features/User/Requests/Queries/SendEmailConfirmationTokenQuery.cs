using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class SendEmailConfirmationTokenQuery : IRequest<Response<string>>
    {
        public SendEmailConfirmationTokenDto SendEmailConfirmationTokenDto { get; set; } = null!;
    }
}
