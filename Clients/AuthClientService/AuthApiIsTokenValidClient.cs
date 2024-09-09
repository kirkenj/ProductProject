using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Clients.AuthApi.AuthApiIStokenValidClient
{
    public interface ITokenValidationClient
    {
        public Task<bool> IsTokenValid(string token);

        public Task<GetHashDefaultsResponce> GetHashDefaultsResponce();
    }

    public class TokenValidationClient : ITokenValidationClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public TokenValidationClient(IHttpClientFactory httpClient, IOptions<AuthClientSettings> options)
        {
            _httpClient = httpClient.CreateClient(nameof(TokenValidationClient));
            _url = options.Value.Uri;
        }

        public async Task<bool> IsTokenValid(string tokenHash)
        {
            var json_ = JsonSerializer.SerializeToUtf8Bytes(tokenHash);
            HttpRequestMessage message = new(HttpMethod.Post, _url + "/api/Tokens/IsTokenValid")
            {
                Content = new ByteArrayContent(json_)
            };
            message.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
            message.Method = new HttpMethod("POST");
            message.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("text/plain"));

            var result = await SendMessage(message);

            var desRes = JsonSerializer.Deserialize<bool>(result.Content.ReadAsStream());

            return desRes;
        }

        public async Task<GetHashDefaultsResponce> GetHashDefaultsResponce()
        {
            HttpRequestMessage message = new(HttpMethod.Get, _url + "/api/Tokens/GetHashDefaults");
            
            var result = await SendMessage(message);

            var desRes = JsonSerializer.Deserialize<GetHashDefaultsResponce>(result.Content.ReadAsStream());

            return desRes ?? throw new ApplicationException("Deserialized value is null");
        }

        private async Task<HttpResponseMessage> SendMessage(HttpRequestMessage message)
        {
            var result = await _httpClient.SendAsync(message);

            var headers_ = new Dictionary<string, IEnumerable<string>>();

            foreach (var item_ in result.Headers)
            {
                headers_[item_.Key] = item_.Value;
            }

            if (!result.IsSuccessStatusCode)
            {
                throw new AuthApiException("The HTTP status code of the response was not expected (" + result.StatusCode + ").", (int)result.StatusCode, await result.Content.ReadAsStringAsync().ConfigureAwait(false), headers_, null);
            }

            return result;
        }
    }
}