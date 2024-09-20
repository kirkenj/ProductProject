using Cache.Contracts;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Cache.Models
{
    public class RedisCustomMemoryCache : ICustomMemoryCache
    {
        private readonly IDatabase _implementation;
        private readonly ConnectionMultiplexer connection;


        public RedisCustomMemoryCache(IOptions<CustomCacheOptions> optionsAccessor)
        {
            connection = ConnectionMultiplexer.Connect(optionsAccessor.Value.ConnectionUri);
            _implementation = connection.GetDatabase();
            
            var keyToCheckConnection = "hello";
            Encoding encoding = Encoding.UTF8;
            _implementation.StringSet(keyToCheckConnection, encoding.GetBytes("World"));
            _implementation.KeyDelete(keyToCheckConnection);
        }

        public Task RemoveAsync(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _implementation.KeyDeleteAsync(key.Trim());
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expirity)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            ArgumentNullException.ThrowIfNull(value, nameof(value));

            var offsetDiff = expirity.TotalMilliseconds;

            if (offsetDiff < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(expirity), $"Offset has to be at least 100 ms (given offset is {offsetDiff})");
            }

            await _implementation.StringSetAsync(key.Trim(), JsonSerializer.Serialize(value), expirity);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var result = await _implementation.StringGetAsync(key.Trim());

            if (result == default || result.HasValue == false || result.IsNullOrEmpty == true)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(result.ToString());
        }
    }
}
