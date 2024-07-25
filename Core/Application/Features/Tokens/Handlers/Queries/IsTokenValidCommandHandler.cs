using Application.DTOs.Tokens;
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
                return await Task.FromResult(Response<bool>.OkResponse(false, string.Empty));
            }

            var key = CacheKeyGenerator.CacheKeyGenerator.KeyForTokenTrackingCaching(request.IsTokenValidDto.Token);
            var trackingResult = _memoryCache.Get(key);

            if (trackingResult == null)
            { 
                return await Task.FromResult(Response<bool>.OkResponse(false, "Token is not tracked"));
            }

            if (trackingResult is not AssignedTokenInfoDto trackInfo)
            {
                throw new InvalidOperationException();
            }

            var banResult = _memoryCache.Get(CacheKeyGenerator.CacheKeyGenerator.KeyForUserTokensInvalidationCaching(trackInfo.UserId.ToString()));
            
            if (banResult == null)
            {
                return await Task.FromResult(Response<bool>.OkResponse(true, string.Empty));
            }

            if (banResult is not DateTime banDateTime)
            {
                throw new InvalidOperationException();
            }

            return await Task.FromResult(Response<bool>.OkResponse(trackInfo.DateTime >= banDateTime, string.Empty));
        }
    }
}
