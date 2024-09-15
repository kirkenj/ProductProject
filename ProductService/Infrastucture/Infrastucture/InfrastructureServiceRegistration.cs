using Application.Contracts.AuthService;
using Cache.Contracts;
using Cache.Models;
using Clients.AuthApi;
using EmailSender.Contracts;
using EmailSender.Models;
using Infrastucture.AuthClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Infrastucture
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.Configure<AuthClientSettings>(configuration.GetSection("AuthClientSettings"));
            services.AddHttpClient();
            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IAuthApiClientService, AuthClientService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            if (isDevelopment)
            {
                services.AddScoped<IEmailSender, ConsoleEmailSender>();
            }
            else
            {
                services.AddSingleton(configuration.GetSection("EmailSettings").Get<EmailSettings>() ?? throw new ArgumentException("Email settings is null"));
                services.AddScoped<IEmailSender, EmailSender.Models.EmailSender>();
            }
            services.Configure<CustomCacheOptions>(configuration.GetSection("RedisCacheOptions"));
            services.AddSingleton<ICustomMemoryCache, RedisAsMemoryCache>();

            return services;
        }
    }
}
