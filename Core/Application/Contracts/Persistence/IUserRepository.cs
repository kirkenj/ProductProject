using Application.Models.User;
using Domain.Models;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericFiltrableRepository<User, Guid, UserFilter>
    {
        public Task<User?> GetAsync(Guid id, bool includeLinks = false);
        public Task<User?> GetAsync(UserFilter filter, bool includeLinks = false);
    }
}
