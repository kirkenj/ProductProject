using Domain.Common.Interfaces;

namespace Application.Contracts.Persistence
{
    public interface IGenericRepository<T, TIdType> where T : IIdObject<TIdType> where TIdType : struct
    {
        public Task<IReadOnlyCollection<T>> GetAllAsync();
        public Task<T?> GetAsync(TIdType id);
        public Task<bool> ExistsAsync(TIdType id);
        public Task AddAsync(T obj);
        public Task DeleteAsync(T obj);
        public Task UpdateAsync(T obj);
    }
}
