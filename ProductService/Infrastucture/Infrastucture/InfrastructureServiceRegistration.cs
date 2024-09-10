using Application.Contracts.Infrastructure;
using Cache.Contracts;
using Cache.Models;
using Clients.AuthApi;
using EmailSender.Contracts;
using EmailSender.Models;
using Infrastucture.AuthClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HashProvider
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>(configuration.GetSection("AuthClientSettings"));
            services.AddHttpClient();
            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IAuthClientService, AuthClientService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddTransient<IEmailSender, EmailSender.Models.EmailSender>();


            services.AddSingleton(new EmailSettings
            {
                ApiAdress = configuration["EmailSettings:ApiAdress"] ?? throw new KeyNotFoundException(),
                ApiPassword = configuration["EmailSettings:ApiPassword"] ?? throw new KeyNotFoundException(),
                ApiLogin = configuration["EmailSettings:ApiLogin"] ?? throw new KeyNotFoundException(),
                ApiPort = int.Parse(configuration["EmailSettings:ApiPort"] ?? throw new KeyNotFoundException()),
                FromName = configuration["EmailSettings:FromName"] ?? throw new KeyNotFoundException(),
                ConsoleMode = bool.Parse(configuration["EmailSettings:ConsoleMode"] ?? throw new KeyNotFoundException())
            });
            services.Configure<CustomCacheOptions>(configuration.GetSection("RedisCacheOptions"));
            services.AddSingleton<ICustomMemoryCache, RedisAsMemoryCache>();

            return services;
        }
    }
}
