using Clients.AuthApi;
using Clients.AuthClientService;
using Clients.ProductApi;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>(configuration.GetSection("AuthClientSettings"));
            services.Configure<ProductClientSettings>(configuration.GetSection("ProductClientSettings"));

            services.AddHttpClient();

            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IProductApiClient, ProductApiClient>();
            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
