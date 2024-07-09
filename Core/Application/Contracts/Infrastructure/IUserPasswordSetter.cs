using System.Security.Cryptography;
using System.Text;

namespace Application.Contracts.Infrastructure
{
    public interface IUserPasswordSetter
    {
        string HashAlgorithmName { get; }
        HashAlgorithm HashFunction { get; }
        Encoding Encoding { get; }

        public void SetPassword(Domain.Models.User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var pwdBytes = Encoding.GetBytes(password);
            var pwdHash = HashFunction.ComputeHash(pwdBytes);

            user.PasswordHash = Encoding.GetString(pwdHash);
            user.StringEncoding = Encoding.EncodingName;
            user.HashAlgorithm = HashFunction.ToString() ?? throw new InvalidOperationException();
        }
    }
}
