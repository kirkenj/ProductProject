using Clients.CustomGateway;
using Front.Services.MessageHandlers;
using Microsoft.Extensions.Options;

namespace Front.Configuration
{
    public static class HttpClientsRegistration
    {
        private const string GATEWAY_API_URI_SECTION_NAME = "GatewayUri";

        public static IServiceCollection ConfigureClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GatewayClientSettings>(o => o.Uri = configuration[GATEWAY_API_URI_SECTION_NAME]?.ToString() ?? throw new InvalidOperationException("Couldn't get Gateway Uri from configuration"));

            services.AddScoped<HeadersMessageHandler>();
            services.AddScoped<TokenDelegatingHandler>();

            services.AddHttpClient<IAuthGatewayClient, GatewayClient>(nameof(IAuthGatewayClient), a => a = new HttpClient());

            services.AddHttpClient<IGatewayClient, GatewayClient>(nameof(IGatewayClient), a => a = new HttpClient())
                .AddHttpMessageHandler<HeadersMessageHandler>()
                .AddHttpMessageHandler<TokenDelegatingHandler>();

            services.AddScoped<IAuthGatewayClient, GatewayClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var client = clf.CreateClient(nameof(IAuthGatewayClient));
                var settings = sp.GetRequiredService<IOptions<GatewayClientSettings>>();
                return new GatewayClient(settings.Value.Uri, client);
            });

            services.AddScoped<IGatewayClient, GatewayClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var client = clf.CreateClient(nameof(IGatewayClient));
                var settings = sp.GetRequiredService<IOptions<GatewayClientSettings>>();
                return new GatewayClient(settings.Value.Uri, client);
            });
            return services;
        }
    }
}
