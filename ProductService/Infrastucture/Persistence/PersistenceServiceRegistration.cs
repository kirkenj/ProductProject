using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Repository.Models;
using Repository.Contracts;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AuthDbConnectionString")));

            return services;
        }
    }
}
