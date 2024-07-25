using Application.Features.Tokens.Requests.Commands;
using Application.Models.Jwt;
using Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Application.Features.Tokens.Handlers.Commands
{
    public class InvalidateUserTokensCommandHandler : IRequestHandler<InvalidateUserTokensCommand, Response<Unit>>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly JwtSettings _jwtSettings;

        public InvalidateUserTokensCommandHandler(IMemoryCache memoryCache, IOptions<JwtSettings> options)
        {
            _memoryCache = memoryCache;
            _jwtSettings = options.Value;
        }

        public async Task<Response<Unit>> Handle(InvalidateUserTokensCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.InvalidateUserTokensDto == null || request.InvalidateUserTokensDto.AssigedTokenInfo == null)
            { 
                return await Task.Run(() => Response<Unit>.BadRequestResponse("Token is null"));
            }

            _memoryCache.Set(
                CacheKeyGenerator.CacheKeyGenerator.KeyForUserTokensInvalidationCaching(request.InvalidateUserTokensDto.AssigedTokenInfo.UserId.ToString()), 
                request.InvalidateUserTokensDto.AssigedTokenInfo.DateTime, 
                DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes));

            return await Task.Run(() => Response<Unit>.OkResponse(Unit.Value, string.Empty));
        }
    }
}
