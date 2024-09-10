using Application.Contracts.Persistence;
using Application.DTOs.User.Interfaces;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class IRoleEditIingDtoValidator : AbstractValidator<IRoleEditIingDto>
    {
        public IRoleEditIingDtoValidator(IRoleRepository roleRepository)
        {
            RuleFor(p => p.RoleID)
                .MustAsync(async (id, cancellationToken) =>
                {
                    var role = await roleRepository.GetAsync(id);
                    return role != null;
                })
                .WithMessage("{PropertyName} does not exist");
        }
    }
}
