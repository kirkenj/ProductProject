using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.JwtAuthentication;
using System.Text;

namespace ProductAPI.JwtAuthentication
{
    public static class JwtAuthenticationRegistration
    {
        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
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
                    //o.EventsType = typeof(CustomJwtBearerEvents);
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

            //services.AddScoped<CustomJwtBearerEvents>();

            return services;
        }
    }
}
