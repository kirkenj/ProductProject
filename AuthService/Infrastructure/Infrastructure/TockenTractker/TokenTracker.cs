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
        public IHashProvider HashProvider { get; private set; }

        public async Task Track(KeyValuePair<string, AssignedTokenInfo<TUserIdType>> pair)
        {
            if (pair.Value == null || pair.Key == null)
                throw new ArgumentNullException(nameof(pair));

            var jwtHash = HashProvider.GetHash(pair.Key);

            var cahceKey = _keyGeneratingDelegate(jwtHash);
            await _memoryCache.SetAsync(
            cahceKey,
                pair.Value,
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes
                ));
        }

        public async Task InvalidateUser(Guid userId, DateTime time)
        {
            await _memoryCache.SetAsync(
                _keyGeneratingDelegate(userId.ToString()),
                time,
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes));

            return;
        }

        public async Task<bool> IsValid(string tokenHash)
        {
            if (tokenHash == null)
            {
                return true;
            }

            var key = _keyGeneratingDelegate(tokenHash);
            var trackInfo = await _memoryCache.GetAsync<AssignedTokenInfo<TUserIdType>>(key) ?? throw new InvalidOperationException("Tocken is not tracked");

            if (trackInfo == null || trackInfo.UserId == null)
            {
                throw new InvalidOperationException(nameof(trackInfo));
            }

            var banResult = await _memoryCache.GetAsync<DateTime>(
                _keyGeneratingDelegate(trackInfo.UserId?.ToString() ?? throw new InvalidOperationException(nameof(trackInfo))));

            if (banResult == default)
            {
                return true;
            }

            return trackInfo.DateTime >= banResult;
        }
    }
}
