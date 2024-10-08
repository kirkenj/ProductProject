using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddDbContext<ProductDbContext>(options =>
            {
                var cString = Environment.GetEnvironmentVariable("ProductDbConnectionString") ?? throw new ArgumentException("Couldn't get connection string");
                options.UseSqlServer(cString);
            });

            using (var scope = services.BuildServiceProvider())
            using (var context = scope.GetService<ProductDbContext>() ?? throw new Exception())
            {
                context.Database.EnsureCreated();
            }

            return services;
        }
    }
}
