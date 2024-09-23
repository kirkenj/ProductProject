using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using System.Text.Json;

namespace Repository.Models
{
    public abstract class GenericFiltrableCachingRepository<T, TIdType, TFilter> : 
        GenericCachingRepository<T, TIdType>, 
        IGenericFiltrableRepository<T, TIdType, TFilter>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableCachingRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachingRepository<T, TIdType>> logger, Func<IQueryable<T>, TFilter, IQueryable<T>> getFilteredSetDelegate)
            : base(dbContext, customMemoryCache, logger)
        {
            GetFilteredSetDelegate = getFilteredSetDelegate;
        }

        private readonly Func<IQueryable<T>, TFilter, IQueryable<T>> GetFilteredSetDelegate;

        public virtual async Task<T?> GetAsync(TFilter filter)
        {
            var key = JsonSerializer.Serialize(filter) + "First";

            _logger.LogInformation($"Got request: {key}");

            var result = await CustomMemoryCache.GetAsync<T>(key);

            if (result != null)
            {
                _logger.LogInformation($"Request {key}. Found in cache");
                return result;
            }

            _logger.LogInformation($"Request {key}. Requesting the database");
            
            result = await GetFilteredSetDelegate(DbSet, filter).FirstOrDefaultAsync();

            if (result != null)
            {
                _ = Task.Run(() => SetCacheAsync(key, result));
            }

            return result;
        }


        public virtual async Task<IReadOnlyCollection<T>> GetPageContent(TFilter filter, int? page = default, int? pageSize = default)
        {
            var key = JsonSerializer.Serialize(filter) + $"page: {page}, pageSize: {pageSize}";

            _logger.LogInformation($"Got request: {key}");

            var result = await CustomMemoryCache.GetAsync<IReadOnlyCollection<T>>(key);

            if (result != null)
            {
                _logger.LogInformation($"Request {key}. Found in cache");
                return result;
            }

            _logger.LogInformation($"Request {key}. Requesting the database");
            result = await GetPageContent(GetFilteredSetDelegate(DbSet, filter), page, pageSize).ToArrayAsync();

            _ = Task.Run(() =>
            {
                _ = Task.Run(() => SetCacheAsync(key, result));

                foreach (var item in result)
                {
                    _ = Task.Run(() => SetCacheAsync(CacheKeyPrefix + item.Id, item));
                }
            });

            return result;
        }
    }
}
