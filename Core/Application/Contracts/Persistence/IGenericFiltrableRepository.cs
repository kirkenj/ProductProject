using Domain.Common.Interfaces;

namespace Application.Contracts.Persistence
{
    public interface IGenericFiltrableRepository<T, TIdType, TFilter> : IGenericRepository<T, TIdType> where T : IIdObject<TIdType> where TIdType : struct
    {
        public Task<IEnumerable<T>> GetRangeAsync(TFilter filter);
        public Task<T?> GetAsync(TFilter filter);
    }
}
