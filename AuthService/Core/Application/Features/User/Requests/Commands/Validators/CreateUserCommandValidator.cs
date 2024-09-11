using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.CreateUserDto).SetValidator(new CreateUserDtoValidator(userRepository));
        }
    }
}
