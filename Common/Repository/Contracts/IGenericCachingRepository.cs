using Cache.Contracts;

namespace Repository.Contracts
{
    public interface IGenericCachingRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected ICustomMemoryCache CustomMemoryCache { get; }
    }
}
