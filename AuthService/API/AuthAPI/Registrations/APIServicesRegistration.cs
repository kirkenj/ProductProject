using AuthAPI.Contracts;
using AuthAPI.Models.Jwt;
using AuthAPI.Models.TokenTracker;


namespace AuthAPI.JwtAuthentication
{
    public static class APIServicesRegistration
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddTransient<IJwtProviderService, JwtProviderService>();


            services.Configure<TokenTrackingSettings>(configuration.GetSection("TokenTrackingSettings"));
            services.AddScoped<ITokenTracker<Guid>, TokenTracker<Guid>>();

            return services;
        }
    }
}
