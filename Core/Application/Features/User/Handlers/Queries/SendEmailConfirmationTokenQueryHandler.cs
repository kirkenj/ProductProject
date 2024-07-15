using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Email;
using Application.Contracts.Infrastructure;
using Application.Features.User.Requests.Queries;

namespace Application.Features.User.Handlers.Queries
{
    public class SendEmailConfirmationTokenQueryHandler : IRequestHandler<SendEmailConfirmationTokenQuery, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;

        public SendEmailConfirmationTokenQueryHandler(IUserRepository userRepository, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.emailSender = emailSender;
        }

        public async Task<string> Handle(SendEmailConfirmationTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.SendEmailConfirmationTokenDto.UserID);

            if (user == null)
            {
                return $"user not found with id = {request.SendEmailConfirmationTokenDto.UserID}";
            }

            if (user.IsEmailConfirmed)
            {
                return "Email is already confirmed";
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return "Email not set";
            }

            var isEmailSent = await emailSender.SendEmailAsync(
                    new Email
                    {
                        Body = $"Dear {user.Login}. Your email is being updated. Your email confirmation status dropped.",
                        Subject = "Email updated",
                        To = user.Email
                    }
                    );

            return isEmailSent ? "Check emails" : "Confirmation token was not sent";
        }
    }
}
