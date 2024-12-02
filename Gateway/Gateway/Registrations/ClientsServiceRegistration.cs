using Clients.AuthApi;
using Clients.AuthClientService;
using Clients.ProductApi;
using Exceptions;
using HttpDelegatingHandlers;
using Microsoft.Extensions.Options;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        private const string HTTTP_CLIENT_NAME = "WithHandler";
        private const string AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME = "AuthApiUri";
        private const string PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME = "ProductApiUri";

        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable(AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME) ?? throw new CouldNotGetEnvironmentVariableException(AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME));
            services.Configure<ProductClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable(PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME) ?? throw new CouldNotGetEnvironmentVariableException(PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME));

            services.AddScoped<AuthHeaderHandler>();
            services.AddHttpClient(HTTTP_CLIENT_NAME).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var cl = clf.CreateClient(HTTTP_CLIENT_NAME);
                var config = sp.GetRequiredService<IOptions<AuthClientSettings>>();
                return new AuthApiClient(config.Value.Uri, cl);
            });

            services.AddScoped<IProductApiClient, ProductApiClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var cl = clf.CreateClient(HTTTP_CLIENT_NAME);
                var config = sp.GetRequiredService<IOptions<ProductClientSettings>>();
                return new ProductApiClient(config.Value.Uri, cl);
            });

            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
