using Application.Contracts.Persistence;
using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            Include(new IUserInfoDtoValidator());

            Include(new IEmailUpdateDtoValidator(userRepository));
        }
    }
}
