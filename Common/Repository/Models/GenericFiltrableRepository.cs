using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericFiltrableRepository<T, TIdType, TFilter> : GenericRepository<T, TIdType>, IGenericFiltrableRepository<T, TIdType, TFilter> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericRepository<T, TIdType>> logger) 
            : this(dbContext.Set<T>(), dbContext.SaveChangesAsync, customMemoryCache, logger)
        {
        }
        public GenericFiltrableRepository(DbSet<T> dbSet, Func<CancellationToken, Task<int>> saveDelegate, ICustomMemoryCache customMemoryCache, ILogger<GenericRepository<T, TIdType>> logger) 
            : base(dbSet, saveDelegate, customMemoryCache, logger)
        {
        }

        protected abstract IQueryable<T> GetFilteredSet(IQueryable<T> set, TFilter filter);

        public virtual async Task<T?> GetAsync(TFilter filter) => await GetFilteredSet(_dbSet, filter).FirstOrDefaultAsync();

        public virtual async Task<IReadOnlyCollection<T>> GetPageContent(TFilter filter, int? page = default, int? pageSize = default)
        {
            var result = await GetPageContent(GetFilteredSet(_dbSet, filter), page, pageSize).ToArrayAsync();

            _ = Task.Run(() =>
            {
                foreach (var item in result)
                {
                    _ = Task.Run(() => SetCache(CacheKeyPrefix + item.Id, item));
                }
            });

            return result;
        }
    }
}
