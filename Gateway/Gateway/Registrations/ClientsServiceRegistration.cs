using Clients;
using Clients.AuthClientService;
using CustomGateway.Clients;
using CustomGateway.Models.AuthClient;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IAuthMicroserviseClient>(configuration.GetSection("AuthClientSettings"));
            services.Configure<ProductClientSettings>(configuration.GetSection("ProductClientSettings"));
            services.AddScoped<HttpClient>((a) => new HttpClient());
            services.AddTransient<IAuthMicroserviseClient, AuthMicroserviseClient>();
            services.AddTransient<IProductClient, ProductClient>();

            return services;
        }
    }
}
