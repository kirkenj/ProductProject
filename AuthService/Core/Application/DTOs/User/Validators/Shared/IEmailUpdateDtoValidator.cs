using Application.Contracts.Persistence;
using Application.DTOs.User.Interfaces;
using Application.Models.User;
using FluentValidation;

namespace Application.DTOs.User.Validators.Shared
{
    public class IEmailUpdateDtoValidator : AbstractValidator<IEmailUpdateDto>
    {
        public IEmailUpdateDtoValidator(IUserRepository userRepository)
        {
            Include(new IEmailDtoValidator());

            RuleFor(p => p.Email)
                .MustAsync(async (Email, cancellationToken) =>
                {
                    var resultUser = await userRepository.GetAsync(new UserFilter { AccurateEmail = Email });
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
