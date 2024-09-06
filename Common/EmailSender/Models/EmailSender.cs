using MimeKit;
using MailKit.Net.Smtp;
using EmailSender.Contracts;

namespace EmailSender.Models
{
    public class EmailSender : IEmailSender
    {
        private EmailSettings Settings { get; }

        public EmailSender(EmailSettings emailSettings)
        {
            Settings = emailSettings;
        }

        public async Task<bool> SendEmailAsync(Email email)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Settings.FromName, Settings.ApiLogin));
            emailMessage.To.Add(new MailboxAddress("User", email.To));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Body
            };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(Settings.ApiAdress, Settings.ApiPort, true);
                await client.AuthenticateAsync(Settings.ApiLogin, Settings.ApiPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
