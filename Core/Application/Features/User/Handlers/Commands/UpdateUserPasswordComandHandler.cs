using Application.DTOs.User.Validators;
using Application.Exceptions;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Contracts.Infrastructure;
using Application.Contracts.Application;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserPasswordComandHandler : IRequestHandler<UpdateUserPasswordComand, Unit>, IPasswordSettingHandler
    {
        private readonly IUserRepository userRepository;

        public UpdateUserPasswordComandHandler(IUserRepository userRepository, IHashProvider hashProvider)
        {
            this.userRepository = userRepository;
            this.HashPrvider = hashProvider;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Unit> Handle(UpdateUserPasswordComand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.UpdateUserPasswordDto.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), $"{nameof(request.UpdateUserPasswordDto.Id)} = {request.UpdateUserPasswordDto.Id}");
            }

            var validator = new UpdateUserPasswordDTOValidator();

            var validaationResult = await validator.ValidateAsync(request.UpdateUserPasswordDto, cancellationToken);

            if (validaationResult.IsValid == false)
            {
                throw new ValidationException(validaationResult);
            }

            var oldPasswordHash = user.PasswordHash;
            if (oldPasswordHash != HashPrvider.GetHash(request.UpdateUserPasswordDto.OldPassword))
            {
                throw new ArgumentException("You sent wrong old password");
            }

            (this as IPasswordSettingHandler).SetPassword(request.UpdateUserPasswordDto.NewPassword, user);

            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
