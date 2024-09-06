using Application.Contracts.Persistence;
using Cache.Contracts;
using Domain.Models;
using Repository.Models;

namespace Persistence.Repositories
{
    public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext, ICustomMemoryCache customMemoryCache) : base(dbContext, customMemoryCache)
        {
            _cacheTimeoutMiliseconds = int.MaxValue;
        }


        public override async Task<IReadOnlyCollection<Role>> GetAllAsync()
        {
            var key = CacheKeyPrefix + "all";

            Console.Write($"Gor request for all {typeof(Role).Name}. ");

            IReadOnlyCollection<Role>? arrRes = _customMemoryCache.Get<IReadOnlyCollection<Role>>(key);

            if (arrRes == null)
            {
                Console.WriteLine("Sending request to the database");
                var dbRes = await base.GetAllAsync();

                foreach (var singleEntry in dbRes) 
                {
                    var singleKey = CacheKeyPrefix + singleEntry.Id;
                    Console.WriteLine("Set cache with key " + singleKey);
                    _customMemoryCache.Set(singleKey, singleEntry, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds));
                }

                _customMemoryCache.Set(key, dbRes, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds));
                Console.WriteLine("Set cache with key " + key);
                return dbRes;
            }

            Console.WriteLine("Found it in cache. Updated cache with key " + key);
            _customMemoryCache.Set(key, arrRes, DateTimeOffset.UtcNow.AddMilliseconds(_cacheTimeoutMiliseconds));

            return arrRes;
        }
    }
}
