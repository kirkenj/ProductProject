using Application.DTOs.Tokens;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Tokens.Requests.Commands
{
    public class InvalidateTokenCommand : IRequest<Response<Unit>>
    {
        public InvalidateTokenDto InvalidateTokenDto { get; set; } = null!;
    }
}
