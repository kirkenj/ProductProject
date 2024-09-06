using Domain.Models;
using Repository.Contracts;

namespace Application.Contracts.Persistence
{
    public interface IRoleRepository : IGenericRepository<Role, int>
    {
    }
}
