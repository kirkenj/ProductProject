using Application.Features.Tokens.Requests.Commands;
using Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Tokens.Handlers.Commands
{
    public class InvalidateTokenCommandHandler : IRequestHandler<InvalidateTokenCommand, Response<Unit>>
    {
        private readonly IMemoryCache _memoryCache;

        public InvalidateTokenCommandHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Response<Unit>> Handle(InvalidateTokenCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.InvalidateTokenDto == null || request.InvalidateTokenDto.Token == null)
            { 
                return await Task.Run(() => Response<Unit>.BadRequestResponse("Token is null"));
            }

            _memoryCache.Set(CacheKeyGenerator.CacheKeyGenerator.KeyForTokenInvalidationCaching(request.InvalidateTokenDto.Token), true, DateTimeOffset.UtcNow.AddHours(1));

            return await Task.Run(() => Response<Unit>.OkResponse(Unit.Value, string.Empty));
        }
    }
}
