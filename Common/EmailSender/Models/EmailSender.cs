using EmailSender.Contracts;
using MailKit.Net.Smtp;
using MimeKit;

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
            WriteMessageToConsole($"Sending email to {email.To}");
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Settings.FromName, Settings.ApiLogin));
            emailMessage.To.Add(new MailboxAddress("User", email.To));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Body
            };

            using var client = new SmtpClient();

            if (Settings.ConsoleMode)
            {
                WriteMessageToConsole($"Message to {email.To}\nSubject: {email.Subject}.\nBody: {email.Body}");
                Thread.Sleep(80);
                return true;
            }

            try
            {
                await client.ConnectAsync(Settings.ApiAdress, Settings.ApiPort, true);
                await client.AuthenticateAsync(Settings.ApiLogin, Settings.ApiPassword);
                await client.SendAsync(emailMessage);

                _ = Task.Run(() => client.DisconnectAsync(true));

                WriteMessageToConsole($"Message to {email.To}. Success");
                return true;
            }
            catch (Exception ex)
            {
                WriteMessageToConsole($"Message to {email.To}. Fail:" + ex.ToString());
                return false;
            }
        }

        private void WriteMessageToConsole(string message)
        {
            Console.WriteLine($"--------------------------------------------------------------------");
            Console.WriteLine($"{nameof(EmailSender)}: {message}");
            Console.WriteLine($"--------------------------------------------------------------------");
        }
    }
}
