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

        protected abstract bool FilterCompareDelegate(T obj, TFilter filter);

        public async Task<T?> GetAsync(TFilter filter) => await _dbSet.FirstOrDefaultAsync(o => FilterCompareDelegate(o, filter));

        public async Task<IEnumerable<T>> GetRangeAsync(TFilter filter) => await _dbSet.Where(o => FilterCompareDelegate(o, filter)).ToArrayAsync();

    }
}
