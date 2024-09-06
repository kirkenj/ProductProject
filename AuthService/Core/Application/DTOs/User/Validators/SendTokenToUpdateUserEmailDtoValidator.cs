using FluentValidation;
using MediatR;

namespace Application.DTOs.User
{
    public class SendTokenToUpdateUserEmailDtoValidator : AbstractValidator<SendTokenToUpdateUserEmailDto>
    {
        public SendTokenToUpdateUserEmailDtoValidator()
        {
            RuleFor(o => o.Email).EmailAddress().NotNull();
        }
    }
}
