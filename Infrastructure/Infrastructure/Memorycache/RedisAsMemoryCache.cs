using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Memorycache
{
    public class RedisAsMemoryCache : RedisCache, ICustomMemoryCache
    {
        public RedisAsMemoryCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        void ICustomMemoryCache.Remove(object key) => RemoveAsync(JsonSerializer.Serialize(key), CancellationToken.None).Wait();

        public void Set<T>(object key, T value, DateTimeOffset offset)
        {
            this.SetString(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpiration = offset });
        }

        public object? Get(object key, Type deserializationType)
        {
            var result = this.GetString(JsonSerializer.Serialize(key));
            return result == null ? null : JsonSerializer.Deserialize(result, deserializationType);
        }
    }
}
