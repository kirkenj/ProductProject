using Application.DTOs.User.Interfaces;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class IUpdateUserPasswordDtoValidator : AbstractValidator<IUpdateUserPasswordDto>
    {
        public IUpdateUserPasswordDtoValidator()
        {
            RuleFor(o => o.NewPassword)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .Matches("^[a-zA-Z0-9]+$").WithMessage("{PropertyName} can contaim A-Z, a-z, 0-9 symbols");
        }
    }
}
