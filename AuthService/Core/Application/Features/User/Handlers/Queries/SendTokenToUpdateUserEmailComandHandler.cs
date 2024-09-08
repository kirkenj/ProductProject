﻿using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Models.Email;
using Application.Models.Response;
using Cache.Contracts;
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
                return Response<string>.BadRequestResponse(validationResult.Errors);
            }

            var newEmail = request.UpdateUserEmailDto.Email;

            var user = await _userRepository.GetAsync(request.UpdateUserEmailDto.Id);

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

            var token = _passwordGenerator.Generate();

            var updateDetails = new EmailUpdateDetails { UserId = user.Id, NewEmail = newEmail };

            var emailSent = await _emailSender.SendEmailAsync(new Email
            {
                To = newEmail,
                Subject = "Change email confirmation",
                Body = $"Confirmation token: '{token}'"
            });

            if (emailSent == false) throw new ApplicationException("Email was not sent");

            _ = Task.Run(() => _memoryCache.SetAsync(CacheKeyGenerator.CacheKeyGenerator.KeyForEmailChangeTokenCaching(token), updateDetails, DateTimeOffset.UtcNow.AddHours(1)), cancellationToken);

            return Response<string>.OkResponse("Check emails to get further details", string.Empty);
        }
    }
}