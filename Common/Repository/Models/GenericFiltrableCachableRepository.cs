using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericFiltrableCachableRepository<T, TIdType, TFilter> : 
        GenericCachableRepository<T, TIdType>, 
        IGenericFiltrableRepository<T, TIdType, TFilter>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableCachableRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachableRepository<T, TIdType>> logger)
            : base(dbContext, customMemoryCache, logger)
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
