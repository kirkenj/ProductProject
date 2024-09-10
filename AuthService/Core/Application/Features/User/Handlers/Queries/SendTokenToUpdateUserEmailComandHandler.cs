using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Models.CacheKeyGenerator;
using Application.Models.Email;
using Cache.Contracts;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class SendTokenToUpdateUserEmailComandHandler : IRequestHandler<SendTokenToUpdateUserEmailRequest, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly ICustomMemoryCache _memoryCache;

        public SendTokenToUpdateUserEmailComandHandler(IUserRepository userRepository, ICustomMemoryCache memoryCache, IEmailSender emailSender, IPasswordGenerator passwordGenerator)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _passwordGenerator = passwordGenerator;
            _memoryCache = memoryCache;
        }

        public async Task<Response<string>> Handle(SendTokenToUpdateUserEmailRequest request, CancellationToken cancellationToken)
        {
            var validator = new SendTokenToUpdateUserEmailDtoValidator();

            var validationResult = validator.Validate(request.UpdateUserEmailDto);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            string newEmail = request.UpdateUserEmailDto.Email;

            Domain.Models.User? user = await _userRepository.GetAsync(request.UpdateUserEmailDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            await _emailSender.SendEmailAsync(new Email
            {
                To = user.Email ?? throw new ApplicationException(),
                Body = $"Dear {user.Name}. Your email is being updated.",
                Subject = "Email is being updated",
            });

            string token = _passwordGenerator.Generate();

            EmailUpdateDetails updateDetails = new() { UserId = user.Id, NewEmail = newEmail };

            bool isEmailSent = await _emailSender.SendEmailAsync(new Email
            {
                To = newEmail,
                Subject = "Change email confirmation",
                Body = $"Confirmation token: '{token}'"
            });

            if (isEmailSent == false) 
            { 
                throw new ApplicationException("Email was not sent"); 
            }

            await _memoryCache.SetAsync(CacheKeyGenerator.KeyForEmailChangeTokenCaching(token), updateDetails, DateTimeOffset.UtcNow.AddHours(1));

            return Response<string>.OkResponse("Check emails to get further details", string.Empty);
        }
    }
}