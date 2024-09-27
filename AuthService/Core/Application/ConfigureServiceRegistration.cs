using Application.Contracts.Persistence;
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
        /// <summary>
        /// Has to be called after repositories configured because this registration calls repository to validate settings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentException"></exception>
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

            var provider = services.BuildServiceProvider();
            var roleRep = provider.GetRequiredService<IRoleRepository>();
            var getRoleTask = roleRep.GetAsync(createUserSettings.DefaultRoleID);
            getRoleTask.Wait();
            if (getRoleTask.Result == null) throw new ArgumentException($"Role with id '{createUserSettings.DefaultRoleID}' doesn't excist.");

            services.Configure<CreateUserSettings>(createUserSettingsSection);
            
            return services;
        }
    }
}
