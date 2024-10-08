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
using System.Text.Json;


namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, bool isDevelopment)
        {
            services.Configure<AuthClientSettings>((s) => s.Uri = Environment.GetEnvironmentVariable("AuthApiUri") ?? throw new ArgumentException("Couldn't get AuthApi Uri"));
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

            services.Configure<CustomCacheOptions>((a) => { a.ConnectionUri = Environment.GetEnvironmentVariable("RedisUri") ?? throw new ArgumentException("Couldn't get RedisUri"); });

            services.AddScoped<ICustomMemoryCache, RedisCustomMemoryCache>();

            return services;
        }
    }
}
