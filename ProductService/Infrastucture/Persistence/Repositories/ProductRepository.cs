using Application.Contracts.Persistence;
using Application.Models.Product;
using Cache.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Models;


namespace Persistence.Repositories
{
    public class ProductRepository : GenericFiltrableRepository<Product, Guid, ProductFilter>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<ProductRepository> logger) : base(dbContext, customMemoryCache, logger)  
        {
        }

        protected override IQueryable<Product> GetFilteredSet(IQueryable<Product> set, ProductFilter filter)
        {
            if (filter == null)
            {
                return set;
            }

            if (filter.Ids != null && filter.Ids.Any())
            {
                set = set.Where(obj => filter.Ids.Contains(obj.Id));
            }

            if (!string.IsNullOrEmpty(filter.NamePart))
            {
                set = set.Where(obj => obj.Name.Contains(filter.NamePart));
            }

            if (!string.IsNullOrEmpty(filter.DescriptionPart))
            {
                set = set.Where(obj => obj.Description.Contains(filter.DescriptionPart));
            }

            if (filter.PriceStart != null)
            {
                set = set.Where(obj => obj.Price >= filter.PriceStart);
            }

            if (filter.PriceEnd != null)
            {
                set = set.Where(obj => obj.Price <= filter.PriceEnd);
            }

            if (filter.IsAvailable != null)
            {
                set = set.Where(obj => obj.IsAvailable == filter.IsAvailable);
            }

            if (filter.ProducerIds != null && filter.ProducerIds.Any())
            {
                set = set.Where(obj => filter.ProducerIds.Contains(obj.ProducerId));
            }

            if (filter.CreationDateStart != null)
            {
                set = set.Where(obj => obj.CreationDate >= filter.CreationDateStart);
            }

            if (filter.CreationDateEnd != null)
            {
                set = set.Where(obj => obj.CreationDate <= filter.CreationDateEnd);
            }

            return set;
        }
    }
}
