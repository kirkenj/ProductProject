using Application.Contracts.Infrastructure;
using Cache.Contracts;
using Cache.Models;
using Clients.AuthClientService;
using EmailSender.Contracts;
using EmailSender.Models;
using Infrastucture.AuthClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthClientSettings>(configuration.GetSection("AuthClientSettings"));
            services.AddScoped<HttpClient>((a) => new HttpClient());
            services.AddScoped<IAuthMicroserviseClient, AuthMicroserviseClient>();
            services.AddScoped<IAuthClientService, AuthClientService>();

            services.AddTransient<IEmailSender, EmailSender.Models.EmailSender>();


            services.AddSingleton(new EmailSettings
            {
                ApiAdress = configuration["EmailSettings:ApiAdress"] ?? throw new ApplicationException(),
                ApiPassword = configuration["EmailSettings:ApiPassword"] ?? throw new ApplicationException(),
                ApiLogin = configuration["EmailSettings:ApiLogin"] ?? throw new ApplicationException(),
                ApiPort = int.Parse(configuration["EmailSettings:ApiPort"] ?? throw new ApplicationException()),
                FromName = configuration["EmailSettings:FromName"] ?? throw new ApplicationException(),
                ConsoleMode = bool.Parse(configuration["EmailSettings:ConsoleMode"] ?? throw new ApplicationException())
            });
            services.Configure<CustomCacheOptions>(configuration.GetSection("RedisCacheOptions"));
            services.AddSingleton<ICustomMemoryCache, RedisAsMemoryCache>();

            return services;
        }
    }
}
