using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Interfaces;
using Infrastructure;

namespace Persistence.Repositories
{
    public abstract class GenericRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly Func<CancellationToken, Task<int>> _saveChangesAsync;

        public GenericRepository(AuthDbContext dbContext)
        { 
            _saveChangesAsync = dbContext.SaveChangesAsync;
            _dbSet = dbContext.Set<T>() ?? throw new Exception("Set not found");
        }

        //protected abstract Action<T, T> Copy(T source, T destination);

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

        public virtual async Task<bool> ExistsAsync(TIdType id) => await _dbSet.AnyAsync(o => o.Id.Equals(id));

        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync() => await _dbSet.ToArrayAsync();

        public virtual async Task<T?> GetAsync(TIdType id) => await _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(id));
        
        public virtual async Task UpdateAsync(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;

            //var destination = await GetAsync(obj.Id);

            //if (destination == null)
            //    return;

            //Copy(obj, destination);

            await _saveChangesAsync(CancellationToken.None);
        }
    }
}
