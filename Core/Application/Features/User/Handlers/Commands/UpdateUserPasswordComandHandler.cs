using Application.DTOs.User.Validators;
using Application.Exceptions;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Contracts.Infrastructure;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserPasswordComandHandler : IRequestHandler<UpdateUserPasswordComand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserPasswordSetter passwordSetter;

        public UpdateUserPasswordComandHandler(IUserRepository userRepository, IUserPasswordSetter userPasswordSetter)
        {
            this.userRepository = userRepository;
            this.passwordSetter = userPasswordSetter;
        }

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

            passwordSetter.SetPassword(user, request.UpdateUserPasswordDTO.Password);

            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
