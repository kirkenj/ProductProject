using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using static System.Net.WebRequestMethods;

namespace Front.MessageHandlers
{
    public class TokenDelegatingHandler : AuthorizationMessageHandler
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public TokenDelegatingHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
        {
            _accessTokenProvider = provider;
            ConfigureHandler(["http://localhost:5118/", "http://localhost:7023"]);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _accessTokenProvider.RequestAccessToken();
            if (token.Status == AccessTokenResultStatus.Success && token.TryGetToken(out AccessToken? accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Value);
                Console.WriteLine("Set access token");
            }
            else
            {
                Console.WriteLine("Couldn't set access token");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
