namespace Cache.Contracts
{
    public interface ICustomMemoryCache
    {
        public Task SetAsync<T>(object key, T value, DateTimeOffset offset);

        public Task RemoveAsync(object key);

        public Task<T?> GetAsync<T>(object key);
    }
}
