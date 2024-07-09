using Application.Models;
using Domain.Models;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericFiltrableRepository<User, Guid, UserFilter>
    {

    }
}
