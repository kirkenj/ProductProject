using Application.Features.Tokens.Requests.Queries;
using Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Tokens.Handlers.Commands
{
    public class IsTokenValidCommandHandler : IRequestHandler<IsTokenValidRequest, Response<bool>>
    {
        private readonly IMemoryCache _memoryCache;

        public IsTokenValidCommandHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Response<bool>> Handle(IsTokenValidRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.IsTokenValidDto == null || request.IsTokenValidDto.Token == null)
            {
                return await Task.Run(() => Response<bool>.OkResponse(true, string.Empty));
            }

            var result = _memoryCache.Get(CacheKeyGenerator.CacheKeyGenerator.KeyForTokenInvalidationCaching(request.IsTokenValidDto.Token));

            return await Task.Run(() => Response<bool>.OkResponse(result == null, string.Empty));
        }
    }
}
