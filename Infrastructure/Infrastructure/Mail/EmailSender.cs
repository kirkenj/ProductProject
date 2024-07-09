using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Infrastructure.Mail
{
    public  class EmailSender : IEmailSender
    {
        private EmailSettings Settings { get; }

        public EmailSender(IOptions<EmailSettings> emailSettings )
        {
            Settings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(Email email)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Settings.FromName, Settings.FromAdress));
            emailMessage.To.Add(new MailboxAddress("User", email.To));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Body
            };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(Settings.FromAdress, Settings.FromPort, true);
                await client.AuthenticateAsync(Settings.ApiLogin, Settings.ApiPassword);//HIDE INTO SECRETS : "kirkend@bk.ru", "aCYaLcDPdhb7Cv8AkYJQ"
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
