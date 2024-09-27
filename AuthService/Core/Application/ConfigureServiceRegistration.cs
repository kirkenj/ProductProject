using Application.MediatRBehaviors;
using Application.Models.User;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ConfigureServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            var createUserSettingsSection = configuration.GetSection("CreateUserSettings");
            var createUserSettings = createUserSettingsSection.Get<CreateUserSettings>() ?? throw new Exception("Got null here");
            var res = createUserSettings.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(createUserSettings));
            if (res.Any()) throw new ValidationException(string.Join("\n", res.Select(r => $"{string.Join(",", r.MemberNames)}: {r.ErrorMessage}")));

            services.Configure<CreateUserSettings>(createUserSettingsSection);

            return services;
        }
    }
}
