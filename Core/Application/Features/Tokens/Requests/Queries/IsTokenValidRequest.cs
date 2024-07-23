using Application.DTOs.Tokens;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Tokens.Requests.Queries
{
    public class IsTokenValidRequest : IRequest<Response<bool>>
    {
        public IsTokenValidDto IsTokenValidDto { get; set; } = null!;
    }
}
