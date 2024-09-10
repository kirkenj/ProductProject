using Application.Models.Product;
using Domain.Models;
using Repository.Contracts;


namespace Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericFiltrableRepository<Product, Guid, ProductFilter>
    {
    }
}
