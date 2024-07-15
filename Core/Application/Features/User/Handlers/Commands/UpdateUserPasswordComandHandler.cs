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

        public UpdateUserPasswordComandHandler(IUserRepository userRepository, IHashProvider userPasswordSetter)
        {
            this.userRepository = userRepository;
            this.PasswordSetter = userPasswordSetter;
        }

        public IHashProvider PasswordSetter { get; private set; }

        public async Task<Unit> Handle(UpdateUserPasswordComand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.UpdateUserPasswordDTO.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), $"{nameof(request.UpdateUserPasswordDTO.Id)} = {request.UpdateUserPasswordDTO.Id}");
            }

            var validator = new UpdateUserPasswordDTOValidator();

            var validaationResult = await validator.ValidateAsync(request.UpdateUserPasswordDTO, cancellationToken);

            if (validaationResult.IsValid == false)
            {
                throw new ValidationException(validaationResult);
            }


            (this as IPasswordSettingHandler).SetPassword(request.UpdateUserPasswordDTO.Password, user);

            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
