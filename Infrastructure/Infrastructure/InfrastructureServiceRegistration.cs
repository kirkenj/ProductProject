using Application.Contracts.Infrastructure;
using Application.Models.Email;
using Application.Models.Hash;
using Application.Models.Jwt;
using Infrastructure.Jwt;
using Infrastructure.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            var validIssuer = configuration["JwtSettings:Issuer"];
            var validAudience = configuration["JwtSettings:Audience"];
            var key = configuration["JwtSettings:Key"] ?? throw new Exception();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = validIssuer,
                        ValidateAudience = true,
                        ValidAudience = validAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });



            return services;
        }
    }
}
