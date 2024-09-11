using Application.DTOs.User.Interfaces;
using FluentValidation;

namespace Application.DTOs.User.Validators.Shared
{
    public class IEmailDtoValidator : AbstractValidator<IEmailDto>
    {
        public IEmailDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty();
        }
    }
}
