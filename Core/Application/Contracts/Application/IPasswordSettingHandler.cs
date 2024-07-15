using Application.Contracts.Infrastructure;
using Domain.Models;

namespace Application.Contracts.Application
{
    public interface IPasswordSettingHandler
    {
        public IHashProvider PasswordSetter { get; }

        public User SetPassword(string password, User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            user.PasswordHash = PasswordSetter.GetHash(password);
            user.StringEncoding = PasswordSetter.Encoding.BodyName;
            user.HashAlgorithm = PasswordSetter.HashAlgorithmName;
            return user;
        }
    }
}
