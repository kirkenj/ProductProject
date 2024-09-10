using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericFiltrableRepository<T, TIdType, TFilter> : GenericRepository<T, TIdType>, IGenericFiltrableRepository<T, TIdType, TFilter> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache) : base(dbContext, customMemoryCache)
        {
        }
        public GenericFiltrableRepository(DbSet<T> dbSet, Func<CancellationToken, Task<int>> saveDelegate, ICustomMemoryCache customMemoryCache) : base(dbSet, saveDelegate, customMemoryCache)
        {
        }

        protected abstract IQueryable<T> GetFilteredSet(IQueryable<T> set, TFilter filter);

        public virtual async Task<T?> GetAsync(TFilter filter) => await GetFilteredSet(_dbSet, filter).FirstOrDefaultAsync();

        public virtual async Task<IReadOnlyCollection<T>> GetPageContent(TFilter filter, int? page = default, int? pageSize = default) =>
            await GetPageContent(GetFilteredSet(_dbSet, filter), page, pageSize).ToArrayAsync();
    }
}
