using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class ConfirmEmailChangeComandValidator : AbstractValidator<ConfirmEmailChangeComand>
    {
        public ConfirmEmailChangeComandValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.ConfirmEmailChangeDto).NotNull().SetValidator(new ConfirmEmailChangeDtoValidator(userRepository));
        }
    }
}