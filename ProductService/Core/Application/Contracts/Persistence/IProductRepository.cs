using Domain.Models;
using Repository.Contracts;


namespace Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
    }
}
