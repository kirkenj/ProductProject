using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Email;
using Application.Contracts.Infrastructure;
using Application.Features.User.Requests.Queries;
using Microsoft.Extensions.Caching.Memory;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class SendEmailConfirmationTokenQueryHandler : IRequestHandler<SendEmailConfirmationTokenQuery, Response<string>>
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

        public async Task<Response<string>> Handle(SendEmailConfirmationTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.SendEmailConfirmationTokenDto.UserID);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(request.SendEmailConfirmationTokenDto.UserID), true);
            }

            if (user.IsEmailConfirmed)
            {
                return Response<string>.BadRequestResponse("Email is already confirmed");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return Response<string>.BadRequestResponse("Email not set");
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
                return Response<string>.OkResponse("Ok", "Check emails");
            }

            return Response<string>.ServerErrorResponse("Confirmation token was not sent");
        }
    }
}
