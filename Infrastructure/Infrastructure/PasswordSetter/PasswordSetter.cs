using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.PasswordSetter
{
    public class PasswordSetter : IUserPasswordSetter
    {
        public IHashProvider HashProvider { get; private set; } = null!;

        public PasswordSetter(IHashProvider hashProvider)
        {
            HashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));
        }
    }
}
