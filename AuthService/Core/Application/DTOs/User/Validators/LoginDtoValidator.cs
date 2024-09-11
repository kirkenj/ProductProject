using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            Include(new IEmailDtoValidator());
            Include(new IPasswordDtoValidator());
        }
    }
}
