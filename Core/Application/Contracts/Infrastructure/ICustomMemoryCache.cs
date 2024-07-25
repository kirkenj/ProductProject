namespace Application.Contracts.Infrastructure
{
    public interface ICustomMemoryCache
    {
        public void Set<T>(object key, T value, DateTimeOffset offset);

        public void Remove(object key);

        public object? Get(object key, Type deserializationType);
    }
}
