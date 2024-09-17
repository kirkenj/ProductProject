namespace Cache.Contracts
{
    public interface ICustomMemoryCache
    {
        public Task SetAsync<T>(string key, T value, DateTimeOffset offset);

        public Task RemoveAsync(string key);

        public Task<T?> GetAsync<T>(string key);
    }
}
