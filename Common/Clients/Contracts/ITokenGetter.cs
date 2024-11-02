namespace Clients.Contracts
{
    public interface ITokenGetter<T>
    {
        public Task<string?> GetToken();
    }
}
