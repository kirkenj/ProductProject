using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("@Server=DESKTOP-89QT4FF\\SQLEXPRESS;Database=AuthDb;TrustServerCertificate=true;Trusted_Connection=True;")));
            //services.AddDbContext<AuthDbContext>(options => options.UseSqlServer("@Server=DESKTOP-89QT4FF\\SQLEXPRESS;Database=AuthDb;TrustServerCertificate=true;Trusted_Connection=True;"));

            return services;
        }
    }
}
