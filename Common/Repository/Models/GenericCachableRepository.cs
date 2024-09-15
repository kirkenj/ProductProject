using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericCachableRepository<T, TIdType> : 
        GenericRepository<T, TIdType>,
        ICachableRepository<T, TIdType>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly ILogger<GenericCachableRepository<T, TIdType>> _logger;

        public GenericCachableRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachableRepository<T, TIdType>> logger) : base(dbContext)
        {
            CustomMemoryCache = customMemoryCache;
            _logger = logger;
        }
        public ICustomMemoryCache CustomMemoryCache { get; private set; }

        public int СacheTimeoutMiliseconds { get; set; } = 2_000;
        protected string CacheKeyPrefix => _repId + " ";

        public override async Task AddAsync(T obj)
        {
            await base.AddAsync(obj);
            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }

        public override async Task DeleteAsync(T obj)
        {
            await base.DeleteAsync(obj);
            var cacheKey = CacheKeyPrefix + obj.Id;
            _ = Task.Run(() => CustomMemoryCache.RemoveAsync(cacheKey));
            _logger.Log(LogLevel.Information, $"Removed the key {cacheKey}");
        }

        public override async Task<IReadOnlyCollection<T>> GetPageContent(int? page = default, int? pageSize = default) 
        {
            var result = await base.GetPageContent(_dbSet, page, pageSize).AsNoTracking().ToArrayAsync();

            _ = Task.Run(() =>
            {
                foreach (var item in result)
                {
                    _ = Task.Run(() => SetCache(CacheKeyPrefix + item.Id, item));
                }
            });
            
            return result;
        }

        public override async Task<T?> GetAsync(TIdType id)
        {
            _logger.Log(LogLevel.Information, $"Got request for {typeof(T).Name} with id = '{id}'. ");
            var cacheKey = CacheKeyPrefix + id;
            var result = await CustomMemoryCache.GetAsync<T>(cacheKey);
            if (result == null)
            {
                _logger.Log(LogLevel.Information, "Sending request to database.");
                result = await base.GetAsync(id);
            }
            else
            {
                _logger.Log(LogLevel.Information, $"Found in it cache.");
            }

            if (result != null)
            {
                _ = Task.Run(() => SetCache(cacheKey, result));
            }

            return result;
        }

        public override async Task UpdateAsync(T obj)
        {
            await base.UpdateAsync(obj);

            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }

        protected virtual async Task SetCache(string key, object value)
        {
            await CustomMemoryCache.SetAsync(key, value, DateTimeOffset.UtcNow.AddMilliseconds(СacheTimeoutMiliseconds));
            _logger.Log(LogLevel.Information, $"Set cache with key '{key}'");
        }
    }
}
