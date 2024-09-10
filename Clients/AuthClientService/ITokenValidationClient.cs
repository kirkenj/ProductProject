using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Clients.AuthApi.AuthApiIStokenValidClient
{
    public interface ITokenValidationClient
    {
        public Task<bool> IsTokenValid(string token);

        public Task<GetHashDefaultsResponce> GetHashDefaultsResponce();
    }

    public class TokenValidationClient : AuthApiClient, ITokenValidationClient
    {
        public TokenValidationClient(IOptions<AuthClientSettings> baseUrl, IHttpClientFactory httpClient, IHttpContextAccessor contextAccessor) : base(baseUrl, httpClient.CreateClient(nameof(ITokenValidationClient)), contextAccessor)
        {

        }

        public Task<GetHashDefaultsResponce> GetHashDefaultsResponce() => GetHashDefaultsAsync();
        

        public Task<bool> IsTokenValid(string token) => IsTokenValidPOSTAsync(token);
    }
}