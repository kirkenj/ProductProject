using Application.Contracts.Persistence;
using Domain.Models;
using Infrastructure;

namespace Persistence.Repositories
{
    public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
