using Application.Contracts.Persistence;
using Domain.Models;
using Repository.Models;

namespace Persistence.Repositories
{
    public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
