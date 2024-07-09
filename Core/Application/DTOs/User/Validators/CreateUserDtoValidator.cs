using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            Include(new IRoleEditIingDtoValidator(roleRepository));

            Include(new IUpdateUserPasswordDtoValidator());

            RuleFor(p => p.Login)
                .MustAsync(async (Login, cancellationToken) =>
                {
                    var resultUser = await userRepository.GetAsync(new Models.UserFilter { AccurateLogin = Login });
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
