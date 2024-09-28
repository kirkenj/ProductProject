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
            UpdateUserEmailDto? cachedDetailsValue = await _memoryCache.GetAsync<UpdateUserEmailDto>(string.Format(_updateUserEmailSettings.UpdateUserEmailCacheKeyFormat, request.ConfirmEmailChangeDto.Token));

            if (cachedDetailsValue == null)
            {
                return Response<string>.NotFoundResponse("No email change requests for this token. Try sending token agan.");
            }
            
            Domain.Models.User? userToUpdate = await _userRepository.GetAsync(cachedDetailsValue.Id);

            if (userToUpdate == null)
            {
                return Response<string>.NotFoundResponse(nameof(Domain.Models.User), true);
            }

            if (userToUpdate.Id != cachedDetailsValue.Id)
            {
                return Response<string>.NotFoundResponse(nameof(request.ConfirmEmailChangeDto.Token), true);
            }

            userToUpdate.Email = cachedDetailsValue.Email;

            await _userRepository.UpdateAsync(userToUpdate);

            await _memoryCache.RemoveAsync(request.ConfirmEmailChangeDto.Token);

            return Response<string>.OkResponse("Email updated.", string.Empty);
        }
    }
}
