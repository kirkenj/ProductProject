using Application.Contracts.Infrastructure;
using Application.DTOs.UserClient;
using AutoMapper;
using Cache.Contracts;
using Clients.AuthClientService;
using System.Text;

namespace Infrastucture.AuthClient
{
    public class AuthClientService : IAuthClientService
    {
        private readonly IAuthMicroserviseClient _authClient;
        private readonly ICustomMemoryCache _customMemoryCache;
        private readonly string cacheKeyPrefix = "ProductAPI_AuthService_";
        private readonly IMapper _mapper;

        public AuthClientService(IAuthMicroserviseClient authClient, ICustomMemoryCache customMemoryCache, IMapper mapper)
        {
            _customMemoryCache = customMemoryCache;
            _authClient = authClient;
            _mapper = mapper;
        }

        public async Task<ClientResponse<bool>> IsTokenValid(string token)
        {
            try
            {
                var result = await _authClient.IsTokenValidPOSTAsync(token);
                return new ClientResponse<bool> { Result = result, Success = true };
            }
            catch (Exception ex)
            {
                return new ClientResponse<bool> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ClientResponse<ICollection<Application.DTOs.UserClient.UserListDto>>?> ListAsync(IEnumerable<Guid>? ids = null, string? accurateLogin = null, string? loginPart = null, string? email = null, string? address = null, IEnumerable<int>? roleIds = null, int? page = null, int? pageSize = null)
        {
            try
            {
                StringBuilder stringBuilder = new(cacheKeyPrefix);
                stringBuilder.Append(ids != null && ids.Any() ? $"Ids: {string.Join(',', ids)}; " : string.Empty);
                stringBuilder.Append(!string.IsNullOrEmpty(accurateLogin) ? $"AccurateLogin: {accurateLogin}; " : string.Empty);
                stringBuilder.Append(!string.IsNullOrEmpty(loginPart) ? $"LoginPart: {loginPart}; " : string.Empty);
                stringBuilder.Append(!string.IsNullOrEmpty(email) ? $"Email: {email}; " : string.Empty);
                stringBuilder.Append(!string.IsNullOrEmpty(address) ? $"Address: {address}; " : string.Empty);
                stringBuilder.Append(roleIds != null && roleIds.Any() ? $"roleIds: {string.Join(',', roleIds)}; " : string.Empty);
                stringBuilder.Append(page.HasValue ? $"page: {page.Value}; " : string.Empty);
                stringBuilder.Append(pageSize.HasValue ? $"pageSize: {pageSize.Value}; " : string.Empty);

                var cacheKey = stringBuilder.ToString();
                Console.WriteLine($"Trying to get a {nameof(Application.DTOs.UserClient.UserListDto)} from {nameof(AuthClientService)} with {stringBuilder}");
                ICollection<Application.DTOs.UserClient.UserListDto> result;
                var a = await _customMemoryCache.GetAsync<ICollection<Application.DTOs.UserClient.UserListDto>>(cacheKey);

                if (a != null)
                {
                    Console.WriteLine("Found it into cache");
                    result = a;
                }
                else
                {
                    Console.WriteLine($"Sending request to {nameof(AuthClientService)}");
                    var qresult = await _authClient.ListAsync(ids, accurateLogin, loginPart, email, address, roleIds, page, pageSize);

                    result = qresult.Select(q => new Application.DTOs.UserClient.UserListDto { Email = q.Email, Id = q.Id, Login = q.Login, Role = _mapper.Map<Application.DTOs.UserClient.RoleDto>(q.Role) }).ToArray();
                }

                _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, DateTimeOffset.UtcNow.AddMilliseconds(10_000)));

                Console.WriteLine("Success");
                return new ClientResponse<ICollection<Application.DTOs.UserClient.UserListDto>> { Result = result, Success = true };
            }
            catch (Exception ex)
            {
                return new ClientResponse<ICollection<Application.DTOs.UserClient.UserListDto>> { Message = ex.Message, Success = false };
            }
        }

        async Task<ClientResponse<GetHashDefaultsResponse>> IAuthClientService.GetHashAlgorithmName()
        {
            try
            {
                return new ClientResponse<GetHashDefaultsResponse> { Result = _mapper.Map<GetHashDefaultsResponse>(await _authClient.GetHashDefaultsAsync()), Success = true };
            }
            catch (Exception ex)
            {
                return new ClientResponse<GetHashDefaultsResponse> { Message = ex.Message, Success = false };
            }
        }

        async Task<ClientResponse<Application.DTOs.UserClient.UserDto?>> IAuthClientService.GetUser(Guid userId)
        {
            try
            {
                var cacheKey = cacheKeyPrefix + "userId_" + userId;

                Console.WriteLine($"Trying to get a {nameof(Application.DTOs.UserClient.UserDto)} from {nameof(AuthClientService)} with {nameof(userId)} = {userId}");
                Application.DTOs.UserClient.UserDto result;
                var a = await _customMemoryCache.GetAsync<Application.DTOs.UserClient.UserDto>(cacheKey);

                if (a != null)
                {
                    Console.WriteLine("Found it into cache");
                    result = a;
                }
                else
                {
                    Console.WriteLine($"Sending request to {nameof(AuthClientService)}");
                    result = _mapper.Map<Application.DTOs.UserClient.UserDto>(await _authClient.UsersGETAsync(userId));
                }

                _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, DateTimeOffset.UtcNow.AddMilliseconds(10_000)));

                Console.WriteLine("Success");
                return new ClientResponse<Application.DTOs.UserClient.UserDto?> { Result = result, Success = true };
            }
            catch (Exception ex)
            {
                return new ClientResponse<Application.DTOs.UserClient.UserDto?> { Message = ex.Message, Success = false };
            }
        }
    }
}
