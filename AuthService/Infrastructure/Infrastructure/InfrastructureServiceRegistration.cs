using Application.Contracts.Infrastructure;
using Infrastructure.HashProvider;
using Cache.Contracts;
using Cache.Models;
using EmailSender.Contracts;
using EmailSender.Models;
using Infrastructure.Jwt;
using Infrastructure.TockenTractker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HashProviderSettings>(configuration.GetSection("HashProviderSettings"));

            services.AddSingleton(new EmailSettings
            {
                ApiAdress = configuration["EmailSettings:ApiAdress"] ?? throw new ApplicationException(),
                ApiPassword = configuration["EmailSettings:ApiPassword"] ?? throw new ApplicationException(),
                ApiLogin = configuration["EmailSettings:ApiLogin"] ?? throw new ApplicationException(),
                ApiPort = int.Parse(configuration["EmailSettings:ApiPort"] ?? throw new ApplicationException()),
                FromName = configuration["EmailSettings:FromName"] ?? throw new ApplicationException(),
                ConsoleMode = bool.Parse(configuration["EmailSettings:ConsoleMode"] ?? throw new ApplicationException())
            });


            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<TokenTrackingSettings>(configuration.GetSection("TokenTrackingSettings"));
            services.Configure<CustomCacheOptions>(configuration.GetSection("CustomCacheOptions"));
            services.AddScoped<ICustomMemoryCache, RedisAsMemoryCache>();
            services.AddScoped<TokenTracker<Guid>>();
            services.AddTransient<IEmailSender, EmailSender.Models.EmailSender>();
            services.AddTransient<IHashProvider, HashProvider.HashProvider>();
            services.AddTransient<IJwtProviderService, JwtProviderService>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator.PasswordGenerator>();

            return services;
        }
    }
}
