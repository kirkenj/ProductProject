using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Commands;
using Application.Models.User;
using Cache.Contracts;
using CustomResponse;
using EmailSender.Contracts;
using MediatR;
using Microsoft.Extensions.Options;


namespace Application.Features.User.Handlers.Commands
{
    public class ConfirmEmailChangeComandHandler : IRequestHandler<ConfirmEmailChangeComand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;
        private readonly UpdateUserEmailSettings _updateUserEmailSettings;

        public ConfirmEmailChangeComandHandler(IUserRepository userRepository, ICustomMemoryCache memoryCache, IEmailSender emailSender, IOptions<UpdateUserEmailSettings> options)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
            _updateUserEmailSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Response<string>> Handle(ConfirmEmailChangeComand request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(_updateUserEmailSettings.UpdateUserEmailCacheKeyFormat, request.ConfirmEmailChangeDto.Token);
            UpdateUserEmailDto? cachedDetailsValue = await _memoryCache.GetAsync<UpdateUserEmailDto>(cacheKey);

            if (cachedDetailsValue == null)
            {
                return Response<string>.BadRequestResponse("Invalid token");
            }

            Domain.Models.User? userToUpdate = await _userRepository.GetAsync(request.ConfirmEmailChangeDto.Id);

            if (userToUpdate == null || userToUpdate.Id != cachedDetailsValue.Id)
            {
                return Response<string>.BadRequestResponse("Invalid token");
            }

            userToUpdate.Email = cachedDetailsValue.Email;

            await _userRepository.UpdateAsync(userToUpdate);

            await _memoryCache.RemoveAsync(cacheKey);

            return Response<string>.OkResponse("Email updated.", string.Empty);
        }
    }
}
