using Application.Contracts.AuthService;
using Clients.AuthApi;
using AutoMapper;
using Cache.Contracts;
using System.Text;
using Microsoft.Extensions.Logging;
using Application.Models.UserClient;

namespace Infrastucture.AuthClient
{
    public class AuthClientService : IAuthApiClientService
    {
        private readonly IAuthApiClient _authClient;
        private readonly ICustomMemoryCache _customMemoryCache;
        private readonly string cacheKeyPrefix = "ProductAPI_AuthService_";
        private readonly IMapper _mapper;
        private readonly ILogger<AuthClientService> _logger;

        public AuthClientService(IAuthApiClient authClient, ICustomMemoryCache customMemoryCache, IMapper mapper, ILogger<AuthClientService> logger)
        {
            _customMemoryCache = customMemoryCache;
            _authClient = authClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ClientResponse<ICollection<AuthClientUser>>?> ListAsync(IEnumerable<Guid>? ids = null, string? accurateLogin = null, string? loginPart = null, string? email = null, string? address = null, IEnumerable<int>? roleIds = null, int? page = null, int? pageSize = null)
        {
            try
            {
                var parametersAsString = StringifyParameters(ids, accurateLogin, loginPart, email, address, roleIds, page, pageSize);

                var cacheKey = cacheKeyPrefix + parametersAsString;
                
                _logger.LogInformation($"Sending request for {nameof(AuthClientUser)} with filter: {parametersAsString}");

                ICollection<AuthClientUser> result;

                var cacheResult = await _customMemoryCache.GetAsync<ICollection<AuthClientUser>>(cacheKey);

                if (cacheResult != null)
                {
                    _logger.LogInformation("Found it into cache");
                    result = cacheResult;
                }
                else
                {
                    _logger.LogInformation($"Sending request to {nameof(AuthClientService)}");

                    var qResult = await _authClient.ListAsync(ids, accurateLogin, loginPart, email, address, roleIds, page, pageSize);

                    result = _mapper.Map<List<AuthClientUser>>(qResult);
                }

                _ = Task.Run(() =>
                {
                    _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, TimeSpan.FromMilliseconds(10_000)));

                    foreach (var item in result)
                    {
                        _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKeyPrefix + "userId_" + item.Id, result, TimeSpan.FromMilliseconds(10_000)));
                    }
                });

                return new ClientResponse<ICollection<AuthClientUser>> { Result = result, Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ClientResponse<ICollection<AuthClientUser>> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ClientResponse<AuthClientUser?>> GetUser(Guid userId)
        {
            try
            {
                var cacheKey = cacheKeyPrefix + "userId_" + userId;

                Console.WriteLine($"Trying to get a {nameof(AuthClientUser)} from {nameof(AuthClientService)} with {nameof(userId)} = {userId}");

                AuthClientUser result;

                var cacheResult = await _customMemoryCache.GetAsync<AuthClientUser>(cacheKey);

                if (cacheResult != null)
                {
                    Console.WriteLine("Found it into cache");
                    result = cacheResult;
                }
                else
                {
                    Console.WriteLine($"Sending request to {nameof(AuthClientService)}");
                    result = _mapper.Map<AuthClientUser>(await _authClient.UsersGETAsync(userId));
                }

                _ = Task.Run(() => _customMemoryCache.SetAsync(cacheKey, result, TimeSpan.FromMilliseconds(10_000)));

                return new ClientResponse<AuthClientUser?> { Result = result, Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ClientResponse<AuthClientUser?> { Message = ex.Message, Success = false };
            }
        }

        private static string StringifyParameters(IEnumerable<Guid>? ids = null, string? accurateLogin = null, string? loginPart = null, string? email = null, string? address = null, IEnumerable<int>? roleIds = null, int? page = null, int? pageSize = null)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(ids != null && ids.Any() ? $"Ids: {string.Join(',', ids)}; " : string.Empty);
            stringBuilder.Append(!string.IsNullOrEmpty(accurateLogin) ? $"AccurateLogin: {accurateLogin}; " : string.Empty);
            stringBuilder.Append(!string.IsNullOrEmpty(loginPart) ? $"LoginPart: {loginPart}; " : string.Empty);
            stringBuilder.Append(!string.IsNullOrEmpty(email) ? $"Email: {email}; " : string.Empty);
            stringBuilder.Append(!string.IsNullOrEmpty(address) ? $"Address: {address}; " : string.Empty);
            stringBuilder.Append(roleIds != null && roleIds.Any() ? $"roleIds: {string.Join(',', roleIds)}; " : string.Empty);
            stringBuilder.Append(page.HasValue ? $"page: {page.Value}; " : string.Empty);
            stringBuilder.Append(pageSize.HasValue ? $"pageSize: {pageSize.Value}; " : string.Empty);

            return stringBuilder.ToString();
        }
    }
}
