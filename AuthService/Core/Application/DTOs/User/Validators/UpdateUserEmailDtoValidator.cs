using Application.Contracts.Persistence;
using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserEmailDtoValidator : AbstractValidator<SendTokenToUpdateUserEmailDto>
    {
        public UpdateUserEmailDtoValidator(IUserRepository userRepository)
        {
            Include(new IEmailUpdateDtoValidator(userRepository));
        }
    }
}
