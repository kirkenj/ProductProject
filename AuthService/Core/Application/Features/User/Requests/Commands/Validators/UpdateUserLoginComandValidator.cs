using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class UpdateUserLoginComandValidator : AbstractValidator<UpdateUserLoginComand>
    {
        public UpdateUserLoginComandValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.UpdateUserLoginDto).NotNull().SetValidator(new UpdateUserLoginDtoValidator(userRepository));
        }
    }
}
