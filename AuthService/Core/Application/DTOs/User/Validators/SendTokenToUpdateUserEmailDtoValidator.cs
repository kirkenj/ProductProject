using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User
{
    public class SendTokenToUpdateUserEmailDtoValidator : AbstractValidator<UpdateUserEmailDto>
    {
        public SendTokenToUpdateUserEmailDtoValidator()
        {
            Include(new IEmailDtoValidator());
            Include(new IIdDtoValidator<Guid>());
        }
    }
}
