using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Clients.AuthApi;
using HashProvider.Contracts;
using HashProvider.Models;

namespace Clients.AuthClientService
{
    public interface ITokenValidationClient
    {
        public Task<bool> IsTokenValid(string token);
    }

    public class TokenValidationClient : AuthApiClient, ITokenValidationClient
    {
        private static IHashProvider? HashProvider;

        public TokenValidationClient(IOptions<AuthClientSettings> baseUrl, IHttpClientFactory httpClient, IHttpContextAccessor contextAccessor) : base(baseUrl, httpClient.CreateClient(nameof(ITokenValidationClient)), contextAccessor)
        {

        }

        private async Task UpdateEncodingAndHashAlgoritm()
        {
            Console.WriteLine($"{nameof(TokenValidationClient)}: Sending request to auth client for hashDefaults.");

            var defaults = await GetHashDefaultsAsync();

            Console.WriteLine($"{nameof(TokenValidationClient)}: Request to auth client for hashDefaults - Success.");

            HashProvider = new HashProvider.Models.HashProvider(new HashProviderSettings { EncodingName = defaults.EncodingName, HashAlgorithmName = defaults.HashAlgorithmName});
        }

        public async Task<bool> IsTokenValid(string token)
        {
            if (HashProvider == null)
            {
                await UpdateEncodingAndHashAlgoritm();
            }

            if (HashProvider == null)
            {
                throw new ApplicationException($"Couldn't get all information about token hashing");
            }

            var tokenHash = HashProvider.GetHash(token);

            return await IsTokenValidPOSTAsync(tokenHash);
        }
    }
}