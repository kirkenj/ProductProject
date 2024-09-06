using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(o => o.Password).NotNull().NotEmpty();
            RuleFor(o => o.Email).EmailAddress().NotEmpty();
        }
    }
}
