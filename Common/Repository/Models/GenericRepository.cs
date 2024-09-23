using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.Models
{
    public class GenericRepository<T, TIdType> : 
        IGenericRepository<T, TIdType>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected static readonly Guid _repId = Guid.NewGuid();

        private DbContext _dbContext = null!;

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        private Func<CancellationToken, Task<int>> SaveChangesAsync => _dbContext.SaveChangesAsync;

        public GenericRepository(DbContext dbContext)
        { 
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task AddAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await _dbContext.AddAsync(obj);

            await SaveChangesAsync(CancellationToken.None);
        }

        public virtual async Task DeleteAsync(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            _dbContext.Remove(obj);
            

            await SaveChangesAsync(CancellationToken.None);
        }


        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync() => await DbSet.ToArrayAsync();

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
            return await GetPageContent(DbSet, page, pageSize).ToArrayAsync();
        }

        public virtual async Task<T?> GetAsync(TIdType id)
        {
            return await DbSet.FirstOrDefaultAsync(o => o.Id.Equals(id));
        }

        public virtual async Task UpdateAsync(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            await SaveChangesAsync(CancellationToken.None);
        }
    }
}
