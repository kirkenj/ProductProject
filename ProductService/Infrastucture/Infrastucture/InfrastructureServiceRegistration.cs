﻿using Application.Contracts.AuthService;
using Cache.Contracts;
using Cache.Models;
using Clients.AuthApi;
using EmailSender.Contracts;
using EmailSender.Models;
using HttpDelegatingHandlers;
using Infrastucture.AuthClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;


namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, bool isDevelopment)
        {
            services.Configure<AuthClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("AuthApiUri") ?? throw new ArgumentException("Couldn't get AuthApi Uri"));

            services.AddScoped<AuthHeaderHandler>();
            
            const string HttpClientName = "WithHandler";
            services.AddHttpClient(HttpClientName).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>(sp =>
            {
                var clf = sp.GetRequiredService<IHttpClientFactory>();
                var cl = clf.CreateClient(HttpClientName);
                var config = sp.GetRequiredService<IOptions<AuthClientSettings>>();
                return new AuthApiClient(config.Value.Uri, cl);
            });

            services.AddScoped<IAuthApiClientService, AuthClientService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            if (isDevelopment)
            {
                services.AddScoped<IEmailSender, ConsoleEmailSender>();
            }
            else
            {
                var EmailSettingsJson = Environment.GetEnvironmentVariable("EmailSettings") ?? throw new ArgumentException("EmailSettings from variables is null");

                var settings = JsonSerializer.Deserialize<EmailSettings>(EmailSettingsJson) ?? throw new ArgumentException("Couldn't deserialize Emailsettings");

                services.Configure<EmailSettings>((a) =>
                {
                    a.FromName = settings.FromName ?? throw new ArgumentException($"Value can not be null ({nameof(settings.FromName)})");
                    if (settings.ApiPort == default) throw new ArgumentException($"Value can not be default ({nameof(settings.FromName)})");
                    a.ApiPort = settings.ApiPort;
                    a.ApiPassword = settings.ApiPassword ?? throw new ArgumentException($"Value can not be null ({nameof(settings.ApiPassword)})");
                    a.ApiLogin = settings.ApiLogin ?? throw new ArgumentException($"Value can not be null ({nameof(settings.ApiLogin)})");
                    a.ApiAdress = settings.ApiAdress ?? throw new ArgumentException($"Value can not be null ({nameof(settings.ApiAdress)})");
                });
                services.AddScoped<IEmailSender, EmailSender.Models.EmailSender>();
            }

            var useDefaultCacheStr = Environment.GetEnvironmentVariable("UseDefaultCache");

            if (useDefaultCacheStr != null && bool.TryParse(useDefaultCacheStr, out bool result) && result)
            {
                services.AddMemoryCache();
                services.AddScoped<ICustomMemoryCache, CustomMemoryCache>();
            }
            else
            {
                services.Configure<CustomCacheOptions>((a) => { a.ConnectionUri = Environment.GetEnvironmentVariable("RedisUri") ?? throw new ArgumentException("Couldn't get RedisUri"); });
                services.AddScoped<ICustomMemoryCache, RedisCustomMemoryCache>();
            }

            return services;
        }
    }
}
