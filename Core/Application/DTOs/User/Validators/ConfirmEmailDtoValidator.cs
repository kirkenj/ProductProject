using FluentValidation;

namespace Application.DTOs.User.Validators
{
    internal class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {
            RuleFor(o => o.UserId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(o => o.Key).NotNull().NotEmpty();
        }
    }
}
