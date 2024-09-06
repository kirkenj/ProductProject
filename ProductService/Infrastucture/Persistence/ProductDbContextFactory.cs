using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Persistence
{
    internal class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ProductDbContext>();
            var connectionString = configuration.GetConnectionString("ProductDbConnectionString");//may be has to be changed

            builder.UseSqlServer(connectionString);

            return new ProductDbContext(builder.Options);
        }
    }
}
