using Application.Contracts.Persistence;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Application.Models.User;

namespace Persistence.Repositories
{
    public class UserRepository : GenericFiltrableRepository<User, Guid, UserFilter>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetAsync(Guid id, bool includeLinks = false) => includeLinks ?
            _dbSet.Include(o => o.Role).FirstOrDefaultAsync(o => o.Id.Equals(id))
            : _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(id));
 
        public Task<User?> GetAsync(UserFilter filter, bool includeLinks = false) => includeLinks ?
            GetFilteredSet(_dbSet, filter).Include(o => o.Role).FirstOrDefaultAsync()
            : GetFilteredSet(_dbSet, filter).FirstOrDefaultAsync();

        protected override IQueryable<User> GetFilteredSet(IQueryable<User> set, UserFilter filter)
        {
            if (filter == null)
            {
                return set;
            }

            #pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            if (!filter.Ids.IsNullOrEmpty())
            {
                set = set.Where(obj => filter.Ids.Contains(obj.Id));
            }

            if (!filter.LoginPart.IsNullOrEmpty())
            {
                set = set.Where(obj => obj.Login.Contains(filter.LoginPart));
            }

            if (!filter.AccurateLogin.IsNullOrEmpty())
            {
                set = set.Where(obj => obj.Login == filter.AccurateLogin);
            }

            if (!filter.Address.IsNullOrEmpty())
            {
                set = set.Where(obj => obj.Address.Contains(filter.Address));
            }

            if (!filter.Email.IsNullOrEmpty())
            {
                #pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                set = set.Where(obj => obj.Email!= null && obj.Email.Contains(filter.Email));
                #pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            }

            if (filter.EmailConfirmed != null)
            {
                set = set.Where(obj => obj.IsEmailConfirmed == filter.EmailConfirmed);
            }
            #pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

            return set;
        }
    }
}
