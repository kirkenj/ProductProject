using Application.Features.Tokens.Requests.Commands;
using Application.Models.Jwt;
using Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Application.Features.Tokens.Handlers.Commands
{
    public class TrackTokenInfoComandHandler : IRequestHandler<TrackTokenInfoComand, Response<Unit>>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly JwtSettings _jwtSettings;

        public TrackTokenInfoComandHandler(IMemoryCache memoryCache, IOptions<JwtSettings> options)
        {
            _memoryCache = memoryCache;
            _jwtSettings = options.Value;
        }

        public Task<Response<Unit>> Handle(TrackTokenInfoComand request, CancellationToken cancellationToken)
        {
            if (request == null || request.KeyValuePair.Value == null || request.KeyValuePair.Key == null) 
                throw new ArgumentNullException(nameof(request));

            var key = CacheKeyGenerator.CacheKeyGenerator.KeyForTokenTrackingCaching(request.KeyValuePair.Key);

            _memoryCache.Set(
                key, 
                request.KeyValuePair.Value, 
                DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes));

            return Task.FromResult(Response<Unit>.OkResponse(Unit.Value, string.Empty));
        }
    }
}
