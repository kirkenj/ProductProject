using Domain.Models;

namespace Application.Contracts.Infrastructure
{
    public interface IUserPasswordSetter
    {
        public IHashProvider HashProvider { get; }

        public User SetPassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            user.PasswordHash = HashProvider.GetHash(password);
            user.StringEncoding = HashProvider.Encoding.BodyName;
            user.HashAlgorithm = HashProvider.HashAlgorithmName;
            return user;
        }
    }
}
