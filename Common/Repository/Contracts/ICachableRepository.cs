using Cache.Contracts;

namespace Repository.Contracts
{
    public interface ICachableRepository<T, TIdType> : IGenericRepository<T, TIdType> where T : class, IIdObject<TIdType> where TIdType : struct
    {
        protected ICustomMemoryCache CustomMemoryCache { get; }

    }
}
