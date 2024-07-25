using Application.DTOs.Tokens;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Tokens.Requests.Commands
{
    public class InvalidateUserTokensCommand : IRequest<Response<Unit>>
    {
        public InvalidateUserTokensDto InvalidateUserTokensDto { get; set; } = null!;
    }
}
