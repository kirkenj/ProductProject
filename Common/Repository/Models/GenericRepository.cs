using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.Models
{
    public abstract class GenericRepository<T, TIdType> : 
        IGenericRepository<T, TIdType>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly Func<CancellationToken, Task<int>> _saveChangesAsync;
        protected static readonly Guid _repId = Guid.NewGuid();

        public GenericRepository(DbContext dbContext)
        { 
            _dbSet = dbContext?.Set<T>() ?? throw new ArgumentNullException(nameof(dbContext));
            _saveChangesAsync = dbContext.SaveChangesAsync;
        }

        public virtual async Task AddAsync(T obj)
        {
            _dbSet.Add(obj);
            await _saveChangesAsync(CancellationToken.None);
        }

        public virtual async Task DeleteAsync(T obj)
        {
            _dbSet.Remove(obj);
            await _saveChangesAsync(CancellationToken.None);
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

        public virtual async Task<IReadOnlyCollection<T>> GetPageContent(int? page = default, int? pageSize = default) 
        {
            return await GetPageContent(_dbSet, page, pageSize).AsNoTracking().ToArrayAsync();
        }

        public virtual async Task<T?> GetAsync(TIdType id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(o => o.Id.Equals(id));
        }

        public virtual async Task UpdateAsync(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
            await _saveChangesAsync(CancellationToken.None);
        }
    }
}
