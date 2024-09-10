using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Clients.AuthApi.AuthApiIStokenValidClient
{
    public interface ITokenValidationClient
    {
        public Task<bool> IsTokenValid(string token);
    }

    public class TokenValidationClient : AuthApiClient, ITokenValidationClient
    {
        private static HashAlgorithm? _hashAlgorithm;
        private static System.Text.Encoding? _hashEncoding;

        public TokenValidationClient(IOptions<AuthClientSettings> baseUrl, IHttpClientFactory httpClient, IHttpContextAccessor contextAccessor) : base(baseUrl, httpClient.CreateClient(nameof(ITokenValidationClient)), contextAccessor)
        {

        }

        private async Task UpdateEncodingAndHashAlgoritm()
        {
            Console.WriteLine($"{nameof(TokenValidationClient)}: Sending request to auth client for hashDefaults.");

            var defaults = await GetHashDefaultsAsync();

            Console.WriteLine($"{nameof(TokenValidationClient)}: Request to auth client for hashDefaults - Success.");

            _hashAlgorithm = HashAlgorithm.Create(defaults.HashAlgorithmName)
                ?? throw new ArgumentException($"Couldn't create {nameof(HashAlgorithm)} with given name: '{defaults.HashAlgorithmName}'");

            _hashEncoding = System.Text.Encoding.GetEncoding(defaults.EncodingName);
        }

        public async Task<bool> IsTokenValid(string token)
        {
            if (_hashAlgorithm == null || _hashEncoding == null)
            {
                await UpdateEncodingAndHashAlgoritm();
            }

            if (_hashAlgorithm == null || _hashEncoding == null)
            {
                throw new ApplicationException($"Couldn't get all information about token hashing");
            }

            _hashAlgorithm.Initialize();

            var tokenHashBytes = _hashAlgorithm.ComputeHash(_hashEncoding.GetBytes(token));

            var tokenHash = _hashEncoding.GetString(tokenHashBytes);

            return await IsTokenValidPOSTAsync(tokenHash);
        }
    }
}