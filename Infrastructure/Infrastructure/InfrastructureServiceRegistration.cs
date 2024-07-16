using Application.Contracts.Infrastructure;
using Application.Models;
using Application.Models.Email;
using Application.Models.Hash;
using Application.Models.Jwt;
using Infrastructure.Jwt;
using Infrastructure.Mail;
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
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IHashProvider, HashProvider.HashProvider>();
            services.AddTransient<IJwtService, JwtService>();

            return services;
        }
    }
}
