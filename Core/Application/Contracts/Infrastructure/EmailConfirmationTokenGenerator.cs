using Application.Models;
using Microsoft.Extensions.Options;

namespace Application.Contracts.Infrastructure
{
    public class EmailConfirmationTokenGenerator
    {
        private readonly EmailConfirmationTokenGeneratorSetting _setting;

        public EmailConfirmationTokenGenerator(IOptions<EmailConfirmationTokenGeneratorSetting> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _setting = options.Value;
        }

        public string GenerateToken(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            return null;
        }

        public bool VerifyToken(string token) { return false; }
    }
}
