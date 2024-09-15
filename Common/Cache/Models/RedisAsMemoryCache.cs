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
        public RedisAsMemoryCache(IOptions<CustomCacheOptions> optionsAccessor) 
            : base(new RedisCacheOptions 
            { 
                Configuration = optionsAccessor.Value.ConnectionUri, 
                InstanceName = optionsAccessor.Value.DockerContainerName
            })
        {
            var keyToCheckConnection = "hello";
            Encoding encoding = Encoding.UTF8;
            this.Set(keyToCheckConnection, encoding.GetBytes("World"));
            this.Remove(keyToCheckConnection);
        }

        public Task RemoveAsync(object key) => RemoveAsync(JsonSerializer.Serialize(key), CancellationToken.None);

        public Task SetAsync<T>(object key, T value, DateTimeOffset offset) => this.SetStringAsync(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions { AbsoluteExpiration = offset });

        public async Task<T?> GetAsync<T>(object key)
        {
            var result = await this.GetStringAsync(JsonSerializer.Serialize(key));
            return result == null ? default : JsonSerializer.Deserialize<T>(result);
        }
    }
}
