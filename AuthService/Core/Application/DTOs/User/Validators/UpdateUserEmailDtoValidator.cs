using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserEmailDtoValidator : AbstractValidator<SendTokenToUpdateUserEmailDto>
    {
        public UpdateUserEmailDtoValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
        }
    }
}
