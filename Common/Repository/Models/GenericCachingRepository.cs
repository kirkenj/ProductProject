using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using System.Collections.Generic;

namespace Repository.Models
{
    public abstract class GenericCachingRepository<T, TIdType> : 
        GenericRepository<T, TIdType>,
        ICachableRepository<T, TIdType>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly ILogger<GenericCachingRepository<T, TIdType>> _logger;

        public GenericCachingRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachingRepository<T, TIdType>> logger) : base(dbContext)
        {
            CustomMemoryCache = customMemoryCache;
            _logger = logger;
        }
        public ICustomMemoryCache CustomMemoryCache { get; private set; }

        public int СacheTimeoutMiliseconds { get; set; } = 2_000;
        protected string CacheKeyPrefix => _repId + " ";

        public override async Task AddAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            
            await base.AddAsync(obj);
            
            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }

        public override async Task DeleteAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await base.DeleteAsync(obj);
            
            var cacheKey = CacheKeyPrefix + obj.Id;

            _ = Task.Run(() => CustomMemoryCache.RemoveAsync(cacheKey));
            
            _logger.Log(LogLevel.Information, $"Removed the key {cacheKey}");
        }

        public override async Task<IReadOnlyCollection<T>> GetPageContent(int? page = default, int? pageSize = default) 
        {
            var key = CacheKeyPrefix + $"(page:{page}, pagesize:{pageSize}";

            var cacheResult = await CustomMemoryCache.GetAsync<IReadOnlyCollection<T>>(key);

            if (cacheResult != null)
            {
                return cacheResult;
            }

            var result = await base.GetPageContent(DbSet, page, pageSize).AsNoTracking().ToArrayAsync();

            _ = Task.Run(() =>
            {
                CustomMemoryCache.SetAsync(key, result, TimeSpan.FromMilliseconds(СacheTimeoutMiliseconds));

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
            
            if (result != null)
            {
                _logger.Log(LogLevel.Information, $"Found in it cache.");
                return result;
            }

            _logger.Log(LogLevel.Information, "Sending request to database.");
            result = await base.GetAsync(id);


            if (result != null)
            {
                _ = Task.Run(() => SetCache(cacheKey, result));
            }

            return result;
        }

        public override async Task UpdateAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            
            await base.UpdateAsync(obj);

            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }

        protected virtual async Task SetCache(string key, object value)
        {
            ArgumentNullException.ThrowIfNull(key);
            
            ArgumentNullException.ThrowIfNull(value);
            
            await CustomMemoryCache.SetAsync(key, value, TimeSpan.FromMilliseconds(СacheTimeoutMiliseconds));
            _logger.Log(LogLevel.Information, $"Set cache with key '{key}'");
        }
    }
}
