using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly Func<CancellationToken, Task<int>> _saveChangesAsync;
        protected readonly ICustomMemoryCache _customMemoryCache;
        protected static readonly Guid _repId = Guid.NewGuid();
        protected int _cacheTimeoutMiliseconds = 2_000;

        public GenericRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache)
        {
            _saveChangesAsync = dbContext.SaveChangesAsync;
            _dbSet = dbContext.Set<T>() ?? throw new Exception("Set not found");
            _customMemoryCache = customMemoryCache;
        }

        public GenericRepository(DbSet<T> dbSet, Func<CancellationToken, Task<int>> saveDelegate, ICustomMemoryCache customMemoryCache)
        {
            _saveChangesAsync = saveDelegate ?? throw new ArgumentNullException(nameof(saveDelegate));
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
            _customMemoryCache = customMemoryCache;
        }

        protected string CacheKeyPrefix => _repId + " ";

        public virtual async Task AddAsync(T obj)
        {
            _dbSet.Add(obj);
            await _saveChangesAsync(CancellationToken.None);
            var cacheKey = CacheKeyPrefix + obj.Id;
            _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, obj, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds)));
            Console.WriteLine($"Set key {cacheKey}");
        }

        public virtual async Task DeleteAsync(T obj)
        {
            _dbSet.Remove(obj);
            await _saveChangesAsync(CancellationToken.None);
            var cacheKey = CacheKeyPrefix + obj.Id;
            _ = Task.Run(() => _customMemoryCache.RemoveAsync(cacheKey));
            Console.WriteLine($"Removed the key {cacheKey}");
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

        public async Task<IReadOnlyCollection<T>> GetPageContent(int? page = default, int? pageSize = default) => await GetPageContent(_dbSet, page, pageSize).AsNoTracking().ToArrayAsync();

        public virtual async Task<T?> GetAsync(TIdType id)
        {
            Console.Write($"Got request for {typeof(T).Name} with id = '{id}'. ");
            var cacheKey = CacheKeyPrefix + id;
            var result = await _customMemoryCache.GetAsync<T>(cacheKey);
            if (result != null)
            {
                Console.WriteLine($"Found in it cache.");
                _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds)));
            }
            else
            {
                Console.WriteLine("Sending request to database.");
                result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(o => o.Id.Equals(id));
            }

            if (result != null)
            {
                _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds)));
                Console.WriteLine($"Set cache with key '{cacheKey}'");
            }

            return result;
        }

        public virtual async Task UpdateAsync(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;

            await _saveChangesAsync(CancellationToken.None);

            var cacheKey = CacheKeyPrefix + obj.Id;

            _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, obj, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds)));
            Console.WriteLine($"Set cache with key '{cacheKey}'");
        }
    }
}
