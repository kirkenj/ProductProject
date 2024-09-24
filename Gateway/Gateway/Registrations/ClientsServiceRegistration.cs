using Clients.AuthApi;
using Clients.AuthClientService;
using Clients.ProductApi;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("AuthApiUri") ?? throw new ArgumentException("AuthApiUri"));
            services.Configure<ProductClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("ProductApiUri") ?? throw new ArgumentException("ProductApiUri"));

            services.AddHttpClient();

            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IProductApiClient, ProductApiClient>();
            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
