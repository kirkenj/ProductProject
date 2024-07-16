using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Email;
using Application.Contracts.Infrastructure;
using Application.Features.User.Requests.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.User.Handlers.Queries
{
    public class SendEmailConfirmationTokenQueryHandler : IRequestHandler<SendEmailConfirmationTokenQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;


        public SendEmailConfirmationTokenQueryHandler(IUserRepository userRepository, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        public async Task<string> Handle(SendEmailConfirmationTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.SendEmailConfirmationTokenDto.UserID);

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

            var token = Guid.NewGuid().ToString();

            var isEmailSent = await _emailSender.SendEmailAsync(
                    new Email
                    {
                        Subject = "Email confirmation",
                        Body = $"Confirmation code: {token}.",
                        To = user.Email
                    }
                    );

            if (isEmailSent) 
            {
                _memoryCache.Set(user.Email, token, DateTimeOffset.UtcNow.AddMinutes(10));
                return "Check emails";
            }
            
            return "Confirmation token was not sent";
        }
    }
}
