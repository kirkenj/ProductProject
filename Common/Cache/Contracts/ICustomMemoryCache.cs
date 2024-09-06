namespace Cache.Contracts
{
    public interface ICustomMemoryCache
    {
        public void Set<T>(object key, T value, DateTimeOffset offset);

        public void Remove(object key);

        public T? Get<T>(object key);
    }
}
