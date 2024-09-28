using Application.Contracts.Persistence;
using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserEmailDtoValidator : AbstractValidator<UpdateUserEmailDto>
    {
        public UpdateUserEmailDtoValidator(IUserRepository userRepository)
        {
            Include(new IEmailUpdateDtoValidator(userRepository));
        }
    }
}
