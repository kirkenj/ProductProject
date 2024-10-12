using Cache.Contracts;
using Microsoft.Extensions.Caching.Memory;


namespace Cache.Models
{
    public class CustomMemoryCache : ICustomMemoryCache
    {
        private readonly IMemoryCache _implementation;

        public CustomMemoryCache(IMemoryCache memoryCache)
        {
            _implementation = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            var objRes = _implementation.Get(key);
            return Task.FromResult(objRes == null ? default : (T)objRes);
        }

        public Task<bool> RefreshKeyAsync(string key, double millisecondsToExpire)
        {
            var objRes = _implementation.Get(key);
            if (objRes == null) 
            {
                return Task.FromResult(false);
            }

            try
            {
                _implementation.Set(key, objRes, DateTimeOffset.UtcNow.AddMilliseconds(millisecondsToExpire));
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task RemoveAsync(string key)
        {
            _implementation.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan offset)
        {
            _implementation.Set(key, value, offset);
            return Task.CompletedTask;
        }
    }
}
