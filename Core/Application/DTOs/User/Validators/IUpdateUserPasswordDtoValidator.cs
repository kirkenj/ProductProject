using Application.DTOs.User.Interfaces;
using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class IUpdateUserPasswordDtoValidator : AbstractValidator<IUpdateUserPasswordDto>
    {
        public IUpdateUserPasswordDtoValidator()
        {
            RuleFor(o => o.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} can not be null or empty")
                .NotNull().WithMessage("{PropertyName} can not be null or empty")
                .MinimumLength(8).WithMessage("{PropertyName} minimal length is {ComparisonValue}")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("{PropertyName} can contaim A-Z, a-z, 0-9 symbols");
        }
    }
}
