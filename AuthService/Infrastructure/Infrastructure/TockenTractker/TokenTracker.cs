using Application.Contracts.Infrastructure;
using Cache.Contracts;
using Microsoft.Extensions.Options;

namespace Infrastructure.TockenTractker
{
    public class TokenTracker<TUserIdType>
    {
        private readonly TokenTrackingSettings _settings = null!;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly Func<string, string> _keyGeneratingDelegate;

        public TokenTracker(IOptions<TokenTrackingSettings> options, ICustomMemoryCache memoryCache, IHashProvider hashProvider)
        {
            _settings = options.Value;
            _memoryCache = memoryCache;
            _keyGeneratingDelegate = (value) => _settings.CacheSeed + value;
            HashProvider = hashProvider;
        }
        public IHashProvider HashProvider {  get; private set; }

        public void Track(KeyValuePair<string, AssignedTokenInfo<TUserIdType>> pair)
        {
            if (pair.Value == null || pair.Key == null)
                throw new ArgumentNullException(nameof(pair));

            var jwtHash = HashProvider.GetHash(pair.Key);

            var cahceKey = _keyGeneratingDelegate(jwtHash);
            _memoryCache.Set(
            cahceKey,
                pair.Value,
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes
                ));

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

        public bool IsValid(string tokenHash)
        {
            if (tokenHash == null)
            {
                return true;
            }

            var key = _keyGeneratingDelegate(tokenHash);
            var trackingResult = _memoryCache.Get<AssignedTokenInfo<TUserIdType>>(key);

            if (trackingResult == null)
            {
                throw new InvalidOperationException("Tocken is not tracked");
            }

            if (trackingResult is not AssignedTokenInfo<TUserIdType> trackInfo)
            {
                throw new InvalidOperationException();
            }

            if (trackInfo == null || trackInfo.UserId == null)
            {
                throw new InvalidOperationException(nameof(trackInfo));
            }

            var banResult = _memoryCache.Get<DateTime>(
                _keyGeneratingDelegate(trackInfo.UserId?.ToString() ?? throw new InvalidOperationException(nameof(trackInfo))));

            if (banResult == default)
            {
                return true;
            }

            return trackInfo.DateTime >= banResult;
        }
    }
}
