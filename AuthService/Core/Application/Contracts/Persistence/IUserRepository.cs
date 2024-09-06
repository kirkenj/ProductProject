using Application.Models.User;
using Domain.Models;
using Repository.Contracts;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericFiltrableRepository<User, Guid, UserFilter>
    {
    }
}
