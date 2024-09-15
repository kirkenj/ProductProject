using Application.Contracts.Persistence;
using Cache.Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Repository.Models;

namespace Persistence.Repositories
{
    public class RoleRepository : GenericCachableRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<RoleRepository> logger) : base(dbContext, customMemoryCache, logger)
        {
            СacheTimeoutMiliseconds = int.MaxValue;
        }

        public override async Task<IReadOnlyCollection<Role>> GetAllAsync()
        {
            var key = CacheKeyPrefix + "all";

            _logger.LogInformation($"Gor request for all instances.");

            IReadOnlyCollection<Role>? arrRes = await CustomMemoryCache.GetAsync<IReadOnlyCollection<Role>>(key);

            if (arrRes == null)
            {
                _logger.LogInformation("Sending request to database for all instances.");
                arrRes = await base.GetAllAsync();
            }
            else
            {
                _logger.LogInformation($"Found it in cache.");
            }

             _logger.LogInformation($"Updated cache with key: '{key}'.");
            _ = Task.Run(() => CustomMemoryCache.SetAsync(key, arrRes, DateTimeOffset.UtcNow.AddMilliseconds(СacheTimeoutMiliseconds)));

            return arrRes;
        }
    }
}
