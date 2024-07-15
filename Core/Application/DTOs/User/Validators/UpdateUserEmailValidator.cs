using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserEmailValidator : AbstractValidator<UpdateUserEmailDto>
    {
        public UpdateUserEmailValidator()
        {
            RuleFor(o => o.Email).EmailAddress();
        }
    }
}
