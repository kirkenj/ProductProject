using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Queries.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(r => r.LoginDto).SetValidator(new LoginDtoValidator());
        }
    }
}
