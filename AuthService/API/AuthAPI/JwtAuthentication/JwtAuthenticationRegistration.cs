using Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAPI.JwtAuthentication
{
    public static class JwtAuthenticationRegistration
    {
        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var settings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new KeyNotFoundException($"Couldn't get {nameof(JwtSettings)}");

            var validIssuer = settings.Issuer ?? throw new KeyNotFoundException(); //configuration["JwtSettings:Issuer"];
            var validAudience = settings.Audience ?? throw new KeyNotFoundException(); //configuration["JwtSettings:Audience"];
            var key = settings.Key ?? throw new KeyNotFoundException(); //configuration["JwtSettings:Key"] ?? throw new KeyNotFoundException();
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
