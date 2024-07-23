using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class ConfirmEmailChangeDtoValidator : AbstractValidator<ConfirmEmailChangeDto>
    {
        public ConfirmEmailChangeDtoValidator()
        {
            RuleFor(o => o.Token).NotNull().NotEmpty();
            RuleFor(o => o.UserId).NotEqual(Guid.Empty);
        }
    }
}
