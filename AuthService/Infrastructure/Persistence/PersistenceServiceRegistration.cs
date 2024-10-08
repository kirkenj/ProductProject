using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddDbContext<AuthDbContext>(options =>
            {
                var cString = Environment.GetEnvironmentVariable("AuthDbConnectionString") ?? throw new ArgumentException("Couldn't get connection string");
                options.UseSqlServer(cString);
            });

            using (var scope = services.BuildServiceProvider())
            using (var context = scope.GetService<AuthDbContext>() ?? throw new Exception())
            {
                context.Database.EnsureCreated();
            }
            return services;
        }
    }
}
