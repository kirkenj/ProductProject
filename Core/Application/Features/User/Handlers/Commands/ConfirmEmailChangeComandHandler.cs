﻿using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Models.Email;
using Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.User.Handlers.Commands
{
    public class ConfirmEmailChangeComandHandler : IRequestHandler<ConfirmEmailChangeComand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailChangeComandHandler(IUserRepository userRepository, IMemoryCache memoryCache, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(ConfirmEmailChangeComand request, CancellationToken cancellationToken)
        {
            var validationResult = new ConfirmEmailChangeDtoValidator().Validate(request.ConfirmEmailChangeDto);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(validationResult.Errors);
            }

            var cachedValue = _memoryCache.Get(CacheKeyGenerator.CacheKeyGenerator.KeyForEmailChangeTokenCaching(request.ConfirmEmailChangeDto.Token));

            if (cachedValue == null)
            {
                return Response<string>.NotFoundResponse("No email change requests for this token. Try sending token agan.");
            }

            if (cachedValue is not EmailUpdateDetails updateDetails)
            {
                throw new ApplicationException();
            }

            var userToUpdate = await _userRepository.GetAsync(updateDetails.UserId);

            if (userToUpdate == null)
            {
                return Response<string>.NotFoundResponse(nameof(Domain.Models.User), true);
            }

            if (userToUpdate.Id != updateDetails.UserId)
            {
                return Response<string>.NotFoundResponse(nameof(request.ConfirmEmailChangeDto.Token), true);
            }

            userToUpdate.Email = updateDetails.NewEmail;
            await _userRepository.UpdateAsync(userToUpdate);

            _memoryCache.Remove(request.ConfirmEmailChangeDto.Token);
            
            return Response<string>.OkResponse("Email updated. Relogin needed", string.Empty);
        }
    }
}
