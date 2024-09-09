using Clients.ProductApi;
using Clients.AuthApi;
using Clients.AuthApi.AuthApiIStokenValidClient;

namespace CustomGateway.Registrations
{
    public static class ClientsServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>(configuration.GetSection("AuthClientSettings"));
            services.Configure<ProductClientSettings>(configuration.GetSection("ProductClientSettings"));
            //services.AddTransient((_) => new HttpClient());

            services.AddHttpClient();

            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<ITokenValidationClient, TokenValidationClient>();

            return services;
        }
    }
}
