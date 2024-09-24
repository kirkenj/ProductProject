using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;

namespace Repository.Models
{
    public class GenericCachingRepository<T, TIdType> : 
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
        private string CacheKeyFormatToAccessSingleViaId => CacheKeyPrefix + "{0}";


        public override async Task AddAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            
            await base.AddAsync(obj);

            await SetCacheAsync(string.Format(CacheKeyFormatToAccessSingleViaId, obj.Id), obj);
        }

        public override async Task DeleteAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await base.DeleteAsync(obj);
            
            var cacheKey = string.Format(CacheKeyFormatToAccessSingleViaId, obj.Id);

            await CustomMemoryCache.RemoveAsync(cacheKey);
            
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

            var result = await base.GetPageContent(DbSet, page, pageSize).ToArrayAsync();

            var tasks = result.Select(r => SetCacheAsync(string.Format(CacheKeyFormatToAccessSingleViaId, r.Id), r))
                .Append(CustomMemoryCache.SetAsync(key, result, TimeSpan.FromMilliseconds(СacheTimeoutMiliseconds)));

            await Task.WhenAll(tasks);
            
            return result;
        }

        public override async Task<T?> GetAsync(TIdType id)
        {
            _logger.Log(LogLevel.Information, $"Got request for {typeof(T).Name} with id = '{id}'. ");
            var cacheKey = string.Format(CacheKeyFormatToAccessSingleViaId, id);
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
                await SetCacheAsync(cacheKey, result);
            }

            return result;
        }

        public override async Task UpdateAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await Task.WhenAll
                (
                    base.UpdateAsync(obj),
                    SetCacheAsync(CacheKeyPrefix + obj.Id, obj)
                );
        }

        public override async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            var key = CacheKeyPrefix + "All";

            var cacheResult = await CustomMemoryCache.GetAsync<IReadOnlyCollection<T>>(key);

            if (cacheResult != null) 
            {
                return cacheResult;
            }

            var result = await base.GetAllAsync();

            await SetCacheAsync(key, result);

            var tasks = result.Select(r => SetCacheAsync(string.Format(CacheKeyFormatToAccessSingleViaId, r.Id), r));

            await Task.WhenAll(tasks);

            return result;
        }

        protected virtual async Task SetCacheAsync(string key, object value)
        {
            ArgumentNullException.ThrowIfNull(key);

            ArgumentNullException.ThrowIfNull(value);

            await CustomMemoryCache.SetAsync(key, value, TimeSpan.FromMilliseconds(СacheTimeoutMiliseconds));

            _logger.Log(LogLevel.Information, $"Set cache with key '{key}'");
        }
    }
}
