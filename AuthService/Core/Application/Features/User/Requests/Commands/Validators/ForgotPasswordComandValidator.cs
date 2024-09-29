using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class ForgotPasswordComandValidator : AbstractValidator<ForgotPasswordComand>
    {
        public ForgotPasswordComandValidator()
        {
            RuleFor(r => r.ForgotPasswordDto).NotNull().SetValidator(new ForgotPasswordDtoValidator());
        }
    }
}
