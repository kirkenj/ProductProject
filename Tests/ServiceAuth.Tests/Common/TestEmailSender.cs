using EmailSender.Models;
using Microsoft.Extensions.Logging;

namespace ServiceAuth.Tests.Common
{
    public class TestEmailSender : ConsoleEmailSender
    {
        public Email LastSentEmail { get; set; } = null!;

        public TestEmailSender(ILogger<EmailSender.Models.EmailSender> logger) : base(logger)
        {
        }

        public override async Task<bool> SendEmailAsync(Email email)
        {
            var emailSent = await base.SendEmailAsync(email);
            if (emailSent)
            {
                LastSentEmail = email;
            }

            return emailSent;
        }
    }
}
