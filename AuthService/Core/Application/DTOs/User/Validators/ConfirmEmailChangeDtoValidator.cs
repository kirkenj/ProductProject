using Application.Contracts.Persistence;
using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class ConfirmEmailChangeDtoValidator : AbstractValidator<ConfirmEmailChangeDto>
    {
        public ConfirmEmailChangeDtoValidator(IUserRepository userRepository)
        {
            Include(new IIdDtoValidator<Guid>());
            RuleFor(u => u.Token).NotEmpty().NotNull();
        }
    }
}
