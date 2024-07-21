using Application.Contracts.Persistence;
using FluentValidation;
using Application.Models.User;

namespace Application.DTOs.User.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            Include(new IUpdateUserPasswordDtoValidator());

            RuleFor(p => p.Login)
                .MustAsync(async (Login, cancellationToken) =>
                {
                    var resultUser = await userRepository.GetAsync(new UserFilter { AccurateLogin = Login });
                    if (resultUser == null)
                    {
                        return true;
                    }

                    return resultUser.Login != Login;
                })
                .WithMessage("{PropertyName} is taken");

            RuleFor(p => p.Login)
                .NotEmpty().WithMessage("{PropertyName} can not be null or empty")
                .NotNull().WithMessage("{PropertyName} can not be null or empty");
        }
    }
}
