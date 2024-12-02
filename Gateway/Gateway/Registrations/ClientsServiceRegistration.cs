using Clients.AuthApi;
using Clients.ProductApi;
using Exceptions;
using HttpDelegatingHandlers;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        private const string HTTTP_CLIENT_NAME = "WithHandler";
        private const string AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME = "AuthApiUri";
        private const string PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME = "ProductApiUri";

        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuthHeaderHandler>();
            services.AddHttpClient(HTTTP_CLIENT_NAME).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>(sp =>
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = clientFactory.CreateClient(HTTTP_CLIENT_NAME);
                var url = Environment.GetEnvironmentVariable(AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME)
                    ?? throw new CouldNotGetEnvironmentVariableException(AUTH_API_URI_ENVIRONMENT_VARIBALE_NAME);

                return new AuthApiClient(url, client);
            });

            services.AddScoped<IProductApiClient, ProductApiClient>(sp =>
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = clientFactory.CreateClient(HTTTP_CLIENT_NAME);
                var url = Environment.GetEnvironmentVariable(PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME)
                    ?? throw new CouldNotGetEnvironmentVariableException(PRODUCT_API_URI_ENVIRONMENT_VARIBALE_NAME);

                return new ProductApiClient(url, client);
            });

            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
