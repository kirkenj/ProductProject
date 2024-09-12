using Application.Models.UserClient;

namespace Application.Contracts.AuthService
{
    public interface IAuthApiClientService
    {
        public Task<ClientResponse<AuthClientUser?>> GetUser(Guid userId);
        public Task<ClientResponse<ICollection<AuthClientUser>>?> ListAsync(IEnumerable<Guid>? ids = null, string? accurateLogin = null, string? loginPart = null, string? email = null, string? address = null, IEnumerable<int>? roleIds = null, int? page = null, int? pageSize = null);
    }
}