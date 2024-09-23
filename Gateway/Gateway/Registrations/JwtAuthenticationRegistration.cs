using CustomGateway.Models.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;

namespace CustomGateway.Registrations
{
    public static class JwtAuthenticationRegistration
    {
        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services)
        {
            var settingsJson = Environment.GetEnvironmentVariable("JwtSettings") ?? throw new ArgumentException("JwtSettings couldn't get from env.variables");

            var settings = JsonSerializer.Deserialize<JwtSettings>(settingsJson) ?? throw new ArgumentException("Couldn't deserialize JwtSettings from env.variables");

            services.Configure<JwtSettings>((configuration) =>
            {
                configuration.Issuer = settings.Issuer ?? throw new ArgumentException(nameof(settings.Issuer) + " is null");
                configuration.Audience = settings.Audience ?? throw new ArgumentException(nameof(settings.Audience) + " is null");
                if (settings.DurationInMinutes == default) throw new ArgumentException(nameof(settings.DurationInMinutes) + $" is {settings.DurationInMinutes.GetType().GetDefaultValue()}");
                configuration.DurationInMinutes = settings.DurationInMinutes;
                configuration.SecurityAlgorithm = settings.SecurityAlgorithm ?? throw new ArgumentException(nameof(settings.SecurityAlgorithm) + " is null");
                configuration.Key = settings.Key ?? throw new ArgumentException(nameof(settings.Key) + " is null");
            });


            var validIssuer = settings.Issuer;
            var validAudience = settings.Audience;
            var key = settings.Key;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.EventsType = typeof(CustomJwtBearerEvents);
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

            services.AddScoped<CustomJwtBearerEvents>();

            return services;
        }
    }
}
