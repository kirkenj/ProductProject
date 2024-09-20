using Application.Contracts.Infrastructure;
using Cache.Contracts;
using Cache.Models;
using EmailSender.Contracts;
using EmailSender.Models;
using HashProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HashProvider.Contracts;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.Configure<HashProviderSettings>(configuration.GetSection("HashProviderSettings"));

            if (isDevelopment)
            {
                services.AddScoped<IEmailSender, ConsoleEmailSender>();
            }
            else
            {
                services.AddSingleton(configuration.GetSection("EmailSettings").Get<EmailSettings>() ?? throw new ArgumentException("Email settings is null"));
                services.AddScoped<IEmailSender, EmailSender.Models.EmailSender>();
            }

            services.Configure<CustomCacheOptions>(configuration.GetSection("CustomCacheOptions"));

            services.AddScoped<ICustomMemoryCache, RedisCustomMemoryCache>();
            services.AddTransient<IHashProvider, HashProvider.Models.HashProvider>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator.PasswordGenerator>();


            return services;
        }
    }
}
