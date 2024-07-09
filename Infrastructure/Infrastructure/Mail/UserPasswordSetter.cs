using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Mail
{
    public class UserPasswordSetter : IUserPasswordSetter
    {
        public UserPasswordSetter(IOptions<PasswordSetterSettings> options)
        {
            if (options.Value == null || options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            HashAlgorithmName = options.Value.HashAlgorithmName;
            HashFunction =  HashAlgorithm.Create(HashAlgorithmName) ?? throw new ArgumentException(nameof(options.Value.HashAlgorithmName));
            Encoding = Encoding.GetEncoding(options.Value.EncodingName) ?? throw new ArgumentException(nameof(options.Value.EncodingName));
        }

        public string HashAlgorithmName { get; private set; } = null!;

        public HashAlgorithm HashFunction { get; private set; } = null!;

        public Encoding Encoding { get; private set; } = null!;
    }
}
