using Application.DTOs.UserClient;

namespace Application.Contracts.Infrastructure
{
    public interface IAuthClientService
    {
        public Task<ClientResponse<UserDto?>> GetUser(Guid userId);
        public Task<ClientResponse<bool>> IsTokenValid(string token);
        public Task<ClientResponse<GetHashDefaultsResponse>> GetHashAlgorithmName();
        public Task<ClientResponse<ICollection<UserListDto>>?> ListAsync(IEnumerable<Guid>? ids = null, string? accurateLogin = null, string? loginPart = null, string? email = null, string? address = null, IEnumerable<int>? roleIds = null, int? page = null, int? pageSize = null);
    }
}
