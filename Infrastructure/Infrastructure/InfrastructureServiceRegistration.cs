using Application.Contracts.Infrastructure;
using Application.Models.Email;
using Application.Models.Hash;
using Application.Models.Jwt;
using Infrastructure.Jwt;
using Infrastructure.Mail;
using Infrastructure.Memorycache;
using Infrastructure.TockenTractker;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HashProviderSettings>(configuration.GetSection("HashProviderSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<TokenTrackingSettings>(configuration.GetSection("TokenTrackingSettings"));
            services.Configure<RedisCacheOptions>(configuration.GetSection("RedisCacheOptions"));

            services.AddSingleton<ICustomMemoryCache, RedisAsMemoryCache>();
            services.AddSingleton<TokenTracker<Guid>>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IHashProvider, HashProvider.HashProvider>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator.PasswordGenerator>();

            return services;
        }
    }
}
