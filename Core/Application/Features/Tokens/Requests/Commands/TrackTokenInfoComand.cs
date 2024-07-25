using Application.DTOs.Tokens;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Tokens.Requests.Commands
{
    public class TrackTokenInfoComand : IRequest<Response<Unit>>
    {
        public KeyValuePair<string, AssignedTokenInfoDto> KeyValuePair { get; set; }
    }
}
