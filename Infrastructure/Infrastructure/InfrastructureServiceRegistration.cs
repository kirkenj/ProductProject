using Application.Contracts.Infrastructure;
using Application.Models;
using Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordSetterSettings>(configuration.GetSection("PasswordSetterSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserPasswordSetter, UserPasswordSetter>();

            return services;
        }
    }
}
