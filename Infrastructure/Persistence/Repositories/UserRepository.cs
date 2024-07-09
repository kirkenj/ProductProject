using Application.Contracts.Persistence;
using Domain.Models;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Application.Models;

namespace Persistence.Repositories
{
    public class UserRepository : GenericFiltrableRepository<User, Guid, UserFilter>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }

        protected override bool FilterCompareDelegate(User obj, UserFilter filter)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            bool idsCmp = filter.Ids.IsNullOrEmpty() || filter.Ids.Contains(obj.Id);

            bool loginPartCmp = filter.LoginPart.IsNullOrEmpty() || obj.Login.Contains(filter.LoginPart);
            
            bool loginCmp = filter.AccurateLogin.IsNullOrEmpty() || obj.Login == filter.AccurateLogin;

            bool addressCmp = filter.Address.IsNullOrEmpty() || obj.Address.Contains(filter.Address);
 
            bool emailCmp = filter.Email.IsNullOrEmpty() || obj.Email == filter.Email;

            bool emilConfirmedCmp = filter.EmailConfirmed == null || obj.IsEmailConfirmed == filter.EmailConfirmed;
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            return idsCmp && loginPartCmp && loginCmp && addressCmp && emailCmp && emilConfirmedCmp;
        }
    }
}
