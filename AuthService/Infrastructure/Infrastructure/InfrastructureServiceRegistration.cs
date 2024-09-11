﻿using Application.Contracts.Infrastructure;
using Cache.Contracts;
using Cache.Models;
using EmailSender.Contracts;
using EmailSender.Models;
using HashProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HashProvider.Contracts;

namespace HashProvider
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HashProviderSettings>(configuration.GetSection("HashProviderSettings"));

            services.AddSingleton(new EmailSettings
            {
                ApiAdress = configuration["EmailSettings:ApiAdress"] ?? throw new KeyNotFoundException(),
                ApiPassword = configuration["EmailSettings:ApiPassword"] ?? throw new KeyNotFoundException(),
                ApiLogin = configuration["EmailSettings:ApiLogin"] ?? throw new KeyNotFoundException(),
                ApiPort = int.Parse(configuration["EmailSettings:ApiPort"] ?? throw new KeyNotFoundException()),
                FromName = configuration["EmailSettings:FromName"] ?? throw new KeyNotFoundException(),
                ConsoleMode = bool.Parse(configuration["EmailSettings:ConsoleMode"] ?? throw new KeyNotFoundException())
            });

            services.Configure<CustomCacheOptions>(configuration.GetSection("CustomCacheOptions"));
            services.AddScoped<ICustomMemoryCache, RedisAsMemoryCache>();
            services.AddScoped<IEmailSender, EmailSender.Models.EmailSender>();
            services.AddTransient<IHashProvider, Models.HashProvider>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator.PasswordGenerator>();

            return services;
        }
    }
}
