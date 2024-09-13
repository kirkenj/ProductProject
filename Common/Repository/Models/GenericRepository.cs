using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly Func<CancellationToken, Task<int>> _saveChangesAsync;
        protected readonly ICustomMemoryCache _customMemoryCache;
        protected static readonly Guid _repId = Guid.NewGuid();
        protected readonly ILogger<GenericRepository<T, TIdType>> _logger;


        public GenericRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericRepository<T, TIdType>> logger) : this(dbContext.Set<T>(), dbContext.SaveChangesAsync, customMemoryCache, logger)
        {
        }

        public GenericRepository(DbSet<T> dbSet, Func<CancellationToken, Task<int>> saveDelegate, ICustomMemoryCache customMemoryCache, ILogger<GenericRepository<T, TIdType>> logger)
        {
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
            _saveChangesAsync = saveDelegate ?? throw new ArgumentNullException(nameof(saveDelegate));
            _customMemoryCache = customMemoryCache;
            _logger = logger;
        }

        public int СacheTimeoutMiliseconds { get; set; } = 2_000;
        protected string CacheKeyPrefix => _repId + " ";

        public virtual async Task AddAsync(T obj)
        {
            _dbSet.Add(obj);
            await _saveChangesAsync(CancellationToken.None);
            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }

        public virtual async Task DeleteAsync(T obj)
        {
            _dbSet.Remove(obj);
            await _saveChangesAsync(CancellationToken.None);
            var cacheKey = CacheKeyPrefix + obj.Id;
            _ = Task.Run(() => _customMemoryCache.RemoveAsync(cacheKey));
            _logger.Log(LogLevel.Information, $"Removed the key {cacheKey}");
        }


        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToArrayAsync();

        protected IQueryable<T> GetPageContent(IQueryable<T> query, int? page = default, int? pageSize = default)
        {
            if (page.HasValue && pageSize.HasValue)
            {
                var pageVal = page.Value <= 0 ? 1 : page.Value;
                var pageSizeVal = pageSize.Value <= 0 ? 1 : pageSize.Value;
                query = query.Skip((pageVal - 1) * pageSizeVal).Take(pageSizeVal);
            }

            return query;
        }

        public async Task<IReadOnlyCollection<T>> GetPageContent(int? page = default, int? pageSize = default) 
        {
            var result = await GetPageContent(_dbSet, page, pageSize).AsNoTracking().ToArrayAsync();

            _ = Task.Run(() =>
            {
                foreach (var item in result)
                {
                    _ = Task.Run(() => SetCache(CacheKeyPrefix + item.Id, item));
                }
            });
            
            return result;
        }

        public virtual async Task<T?> GetAsync(TIdType id)
        {
            _logger.Log(LogLevel.Information, $"Got request for {typeof(T).Name} with id = '{id}'. ");
            var cacheKey = CacheKeyPrefix + id;
            var result = await _customMemoryCache.GetAsync<T>(cacheKey);
            if (result == null)
            {
                _logger.Log(LogLevel.Information, "Sending request to database.");
                result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(o => o.Id.Equals(id));
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

        public virtual async Task UpdateAsync(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;

            await _saveChangesAsync(CancellationToken.None);

            _ = Task.Run(() => SetCache(CacheKeyPrefix + obj.Id, obj));
        }


        protected virtual async Task SetCache(string key, object value)
        {
            await _customMemoryCache.SetAsync(key, value, DateTimeOffset.UtcNow.AddMilliseconds(СacheTimeoutMiliseconds));
            _logger.Log(LogLevel.Information, $"Set cache with key '{key}'");
        }
    }
}
