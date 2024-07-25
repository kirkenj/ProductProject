using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.TockenTractker
{
    public class TokenTracker
    {
        private readonly TokenTrackingSettings _settings = null!;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly Func<string, string> _keyGeneratingDelegate;

        public TokenTracker(IOptions<TokenTrackingSettings> options, ICustomMemoryCache memoryCache)
        {
            _settings = options.Value;
            _memoryCache = memoryCache;
            _keyGeneratingDelegate = (value) => _settings.CacheSeed + value;
        }

        public void Track(KeyValuePair<string, AssignedTokenInfo> pair)
        {
            if (pair.Value == null || pair.Key == null)
                throw new ArgumentNullException(nameof(pair));

            var key = _keyGeneratingDelegate(pair.Key);
            _memoryCache.Set(
            key,
                pair.Value,
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes));

            return;
        }

        public void InvalidateUser(Guid userId, DateTime time)
        {
            _memoryCache.Set(
                _keyGeneratingDelegate(userId.ToString()),
                time,
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes));

            return;
        }

        public bool IsValid(string token)
        {
            if (token == null)
            {
                return true;
            }

            var key = _keyGeneratingDelegate(token);
            var trackingResult = _memoryCache.Get(key, typeof(AssignedTokenInfo));

            if (trackingResult == null)
            {
                throw new InvalidOperationException("Tocken is not tracked");
            }

            if (trackingResult is not AssignedTokenInfo trackInfo)
            {
                throw new InvalidOperationException();
            }

            var banResult = _memoryCache.Get(_keyGeneratingDelegate(trackInfo.UserId.ToString()), typeof(DateTime));

            if (banResult == null)
            {
                return true;
            }

            if (banResult is not DateTime banDateTime)
            {
                throw new InvalidOperationException();
            }

            return trackInfo.DateTime >= banDateTime;
        }
    }
}
