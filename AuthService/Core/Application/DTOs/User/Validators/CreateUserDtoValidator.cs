using Application.Contracts.Persistence;
using Application.Models.User;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();

            RuleFor(x => x.Address).NotEmpty().NotNull();

            RuleFor(p => p.Email)
                .MustAsync(async (Email, cancellationToken) =>
                {
                    var resultUser = await userRepository.GetAsync(new UserFilter { Email = Email });
                    if (resultUser == null)
                    {
                        return true;
                    }

                    return resultUser.Email != Email;
                })
                .WithMessage("{PropertyName} is taken");
        }
    }
}
