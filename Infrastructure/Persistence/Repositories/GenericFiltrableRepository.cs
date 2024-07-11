using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Interfaces;
using Infrastructure;

namespace Persistence.Repositories
{
    public abstract class GenericFiltrableRepository<T, TIdType, TFilter> : GenericRepository<T, TIdType>, IGenericFiltrableRepository<T, TIdType, TFilter> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }

        protected abstract IQueryable<T> GetFilteredSet(IQueryable<T> set, TFilter filter);

        public async Task<T?> GetAsync(TFilter filter) => await GetFilteredSet(_dbSet, filter).FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetRangeAsync(TFilter filter) => await GetFilteredSet(_dbSet, filter).ToArrayAsync();
    }
}
