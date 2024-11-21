using Clients.AuthApi;
using Clients.AuthClientService;
using Clients.ProductApi;
using HttpDelegatingHandlers;
using Microsoft.Extensions.Options;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("AuthApiUri") ?? throw new ArgumentException("AuthApiUri"));
            services.Configure<ProductClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("ProductApiUri") ?? throw new ArgumentException("ProductApiUri"));

            const string httpClientName = "WithHandler";
            services.AddScoped<AuthHeaderHandler>();
            services.AddHttpClient(httpClientName).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var cl = clf.CreateClient(httpClientName);
                var config = sp.GetRequiredService<IOptions<AuthClientSettings>>();
                return new AuthApiClient(config.Value.Uri, cl);
            });

            services.AddScoped<IProductApiClient, ProductApiClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var cl = clf.CreateClient(httpClientName);
                var config = sp.GetRequiredService<IOptions<ProductClientSettings>>();
                return new ProductApiClient(config.Value.Uri, cl);
            });

            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
