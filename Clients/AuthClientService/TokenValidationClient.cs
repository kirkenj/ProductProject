using Clients.AuthApi;
using HashProvider.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Clients.AuthClientService
{
    public interface ITokenValidationClient
    {
        public Task<bool> IsTokenValid(string token);
    }

    public class TokenValidationClient : AuthApiClient, ITokenValidationClient
    {
        private static IHashProvider? HashProvider;
        private static ILogger Logger = null!;

        public TokenValidationClient(IOptions<AuthClientSettings> baseUrl, IHttpClientFactory httpClient, IHttpContextAccessor contextAccessor, ILogger<TokenValidationClient> logger) : base(baseUrl, httpClient.CreateClient(nameof(ITokenValidationClient)), contextAccessor)
        {
            Logger = logger;
        }

        private async Task UpdateEncodingAndHashAlgoritm()
        {
            Logger.LogInformation($"Sending request to auth client for hashDefaults.");

            var defaults = await GetHashDefaultsAsync();

            Logger.LogInformation($"Request to auth client for hashDefaults - Success.");

            HashProvider = new HashProvider.Models.HashProvider(new HashProvider.Models.HashProviderSettings { EncodingName = defaults.EncodingName, HashAlgorithmName = defaults.HashAlgorithmName });
        }

        public async Task<bool> IsTokenValid(string token)
        {
            if (HashProvider == null)
            {
                await UpdateEncodingAndHashAlgoritm();
            }

            var tokenHash = HashProvider?.GetHash(token) ?? throw new ApplicationException($"Couldn't get hash");

            return await IsTokenValidAsync(tokenHash);
        }
    }
}