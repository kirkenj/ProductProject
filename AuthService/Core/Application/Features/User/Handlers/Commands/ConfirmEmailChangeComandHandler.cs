using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using Application.Models.CacheKeyGenerator;
using Application.Models.Email;
using Cache.Contracts;
using CustomResponse;
using EmailSender.Contracts;
using MediatR;


namespace Application.Features.User.Handlers.Commands
{
    public class ConfirmEmailChangeComandHandler : IRequestHandler<ConfirmEmailChangeComand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailChangeComandHandler(IUserRepository userRepository, ICustomMemoryCache memoryCache, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(ConfirmEmailChangeComand request, CancellationToken cancellationToken)
        {
            EmailUpdateDetails? cachedDetailsValue = await _memoryCache.GetAsync<EmailUpdateDetails>(CacheKeyGenerator.KeyForEmailChangeTokenCaching(request.ConfirmEmailChangeDto.Token));

            if (cachedDetailsValue == null)
            {
                return Response<string>.NotFoundResponse("No email change requests for this token. Try sending token agan.");
            }

            Domain.Models.User? userToUpdate = await _userRepository.GetAsync(cachedDetailsValue.UserId);

            if (userToUpdate == null)
            {
                return Response<string>.NotFoundResponse(nameof(Domain.Models.User), true);
            }

            if (userToUpdate.Id != cachedDetailsValue.UserId)
            {
                return Response<string>.NotFoundResponse(nameof(request.ConfirmEmailChangeDto.Token), true);
            }

            userToUpdate.Email = cachedDetailsValue.NewEmail;

            await _userRepository.UpdateAsync(userToUpdate);

            await _memoryCache.RemoveAsync(request.ConfirmEmailChangeDto.Token);

            return Response<string>.OkResponse("Email updated.", string.Empty);
        }
    }
}
