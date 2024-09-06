using Cache.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Cache.Models
{
    public class RedisAsMemoryCache : RedisCache, ICustomMemoryCache
    {
        public RedisAsMemoryCache(IOptions<CustomCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        void ICustomMemoryCache.Remove(object key) => RemoveAsync(JsonSerializer.Serialize(key), CancellationToken.None).Wait();

        public void Set<T>(object key, T value, DateTimeOffset offset)
        {
            this.SetString(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpiration = offset });
        }

        public T? Get<T>(object key)
        {
            var result = this.GetString(JsonSerializer.Serialize(key));
            return result == null ? default : JsonSerializer.Deserialize<T>(result);
        }
    }
}
