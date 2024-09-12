using Application.Contracts.Persistence;
using Application.Models.User;
using Cache.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Models;

namespace Persistence.Repositories
{
    public class UserRepository : GenericFiltrableRepository<User, Guid, UserFilter>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<UserRepository> logger) : base(dbContext, customMemoryCache, logger)
        {
            _cacheTimeoutMiliseconds = 10000;
        }

        protected override IQueryable<User> GetFilteredSet(IQueryable<User> set, UserFilter filter)
        {
            if (filter == null)
            {
                return set;
            }

            if (filter.Ids != null && filter.Ids.Any())
            {
                set = set.Where(obj => filter.Ids.Contains(obj.Id));
            }

            if (filter.RoleIds != null && filter.RoleIds.Any())
            {
                set = set.Where(obj => filter.RoleIds.Contains(obj.RoleID));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                set = set.Where(obj => obj.Address.Contains(filter.Address));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                set = set.Where(obj => obj.Email != null && obj.Email.Contains(filter.Email));
            }

            if (!string.IsNullOrEmpty(filter.LoginPart))
            {
                set = set.Where(obj => obj.Login != null && obj.Login.Contains(filter.LoginPart));
            }

            if (!string.IsNullOrEmpty(filter.AccurateLogin))
            {
                set = set.Where(obj => obj.Login != null && obj.Login == filter.AccurateLogin);
            }

            return set;
        }
    }
}
