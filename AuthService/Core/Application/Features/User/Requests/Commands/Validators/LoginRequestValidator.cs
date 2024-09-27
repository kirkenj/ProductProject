using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(r => r.LoginDto).NotNull().SetValidator(new LoginDtoValidator());
        }
    }
}
