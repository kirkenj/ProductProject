using Cache.Contracts;
using Cache.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cache.Tests.Models
{
    public class TestBase
    {
        protected readonly ICustomMemoryCache _cache = null!;

        public TestBase(Type type)
        {
            if (type == typeof(RedisCustomMemoryCache))
            {

                CustomCacheOptions customCacheOptions = new()
                {
                    ConnectionUri = "localhost:3330"
                };

                IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

                _cache = new RedisCustomMemoryCache(options);
            }
            else if (type == typeof(CustomMemoryCache))
            {
                _cache = new CustomMemoryCache(new MemoryCache(new MemoryCacheOptions()));
            }
        }
    }
}
