using Application.Contracts.Persistence;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        private const string DATABASE_CONNECTION_STRING_ENVIRONMENT_VARIBALE_NAME = "AuthDbConnectionString";

        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddDbContext<AuthDbContext>(options =>
            {
                var cString = Environment.GetEnvironmentVariable(DATABASE_CONNECTION_STRING_ENVIRONMENT_VARIBALE_NAME)
                    ?? throw new CouldNotGetEnvironmentVariableException(DATABASE_CONNECTION_STRING_ENVIRONMENT_VARIBALE_NAME);
                options.UseSqlServer(cString);
            });

            using var scope = services.BuildServiceProvider();
            using var context = scope.GetRequiredService<AuthDbContext>();
            context.Database.EnsureCreated();
            return services;
        }
    }
}
