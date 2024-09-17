using Cache.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Cache.Models
{
    public class RedisAsMemoryCache : ICustomMemoryCache
    {
        private RedisCache _implementation;


        public RedisAsMemoryCache(IOptions<CustomCacheOptions> optionsAccessor)
        {
            _implementation = new RedisCache(new RedisCacheOptions
            {
                Configuration = optionsAccessor.Value.ConnectionUri
            });


            var keyToCheckConnection = "hello";
            Encoding encoding = Encoding.UTF8;
            _implementation.Set(keyToCheckConnection, encoding.GetBytes("World"));
            _implementation.Remove(keyToCheckConnection);
        }

        public Task RemoveAsync(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = key.Trim();

            return _implementation.RemoveAsync(key, CancellationToken.None);
        }

        public async Task SetAsync<T>(string key, T value, DateTimeOffset offset)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = key.Trim();

            ArgumentNullException.ThrowIfNull(value, nameof(value));

            var offsetDiff = (offset - DateTimeOffset.Now).Milliseconds;

            if (offsetDiff < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset has to be at least 100 ms ahead from now (given offset is {offsetDiff})");
            }

            await _implementation.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpiration = offset });
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = key.Trim();

            var result = await _implementation.GetStringAsync(key);

            return result == null ? default : JsonSerializer.Deserialize<T>(result);
        }
    }
}
