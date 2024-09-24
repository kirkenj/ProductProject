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
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddDbContext<ProductDbContext>(options =>
            {
                var cString = Environment.GetEnvironmentVariable("ProductDbConnectionString") ?? throw new ArgumentException("Couldn't get connection string");
                options.UseSqlServer(Environment.GetEnvironmentVariable(cString));
            });

            return services;
        }
    }
}
