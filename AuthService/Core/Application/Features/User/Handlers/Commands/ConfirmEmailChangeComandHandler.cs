using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Models.CacheKeyGenerator;
using Application.Models.Email;
using Application.Models.Response;
using Cache.Contracts;
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
            FluentValidation.Results.ValidationResult validationResult = new ConfirmEmailChangeDtoValidator().Validate(request.ConfirmEmailChangeDto);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(validationResult.Errors);
            }

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
