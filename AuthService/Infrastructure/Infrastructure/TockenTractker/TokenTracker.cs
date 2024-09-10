﻿using Application.Contracts.Infrastructure;
using Application.Models.TokenTracker;
using Cache.Contracts;
using Microsoft.Extensions.Options;

namespace Infrastructure.TokenTractker
{
    public class TokenTracker<TUserIdType> : ITokenTracker<TUserIdType> where TUserIdType : struct
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

        public async Task InvalidateUser(TUserIdType userId, DateTime time)
        {
            if (userId.Equals(default))
            {
                throw new ArgumentException($"{nameof(userId)} can not be {default(TUserIdType)}", nameof(userId));
            }

            string userIdStr = userId.ToString() ?? throw new ApplicationException("Couldn't get user's id as string");

            await _memoryCache.SetAsync(
                _keyGeneratingDelegate(userIdStr),
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
            var trackInfo = await _memoryCache.GetAsync<AssignedTokenInfo<TUserIdType>>(key) ?? throw new InvalidOperationException("Token is not tracked");

            if (trackInfo == null || trackInfo.UserId.Equals(default))
            {
                throw new InvalidOperationException(nameof(trackInfo));
            }

            var banResult = await _memoryCache.GetAsync<DateTime>(
                _keyGeneratingDelegate(trackInfo.UserId.ToString() ?? throw new InvalidOperationException(nameof(trackInfo))));

            if (banResult == default)
            {
                return true;
            }

            return trackInfo.DateTime >= banResult;
        }

        public async Task Track(string token, TUserIdType userId, DateTime tokenRegistrationTime)
        {
            var jwtHash = HashProvider.GetHash(token ?? throw new ArgumentNullException(nameof(token)));

            var cahceKey = _keyGeneratingDelegate(jwtHash);
            await _memoryCache.SetAsync(
                cahceKey,
                new AssignedTokenInfo<TUserIdType> { DateTime = tokenRegistrationTime, UserId = userId },
                DateTimeOffset.UtcNow.AddMinutes(_settings.DurationInMinutes
                ));
        }
    }
}
