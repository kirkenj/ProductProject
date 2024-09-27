using Application.Contracts.Persistence;
using Cache.Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Repository.Models;

namespace Persistence.Repositories
{
    public class RoleRepository : GenericCachingRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachingRepository<Role, int>> logger) : base(dbContext, customMemoryCache, logger)
        {
            this.СacheTimeoutMiliseconds = int.MaxValue;
        }
    }
}
