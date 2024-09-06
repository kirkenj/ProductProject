using Application.DTOs.User.Interfaces;
using Application.Contracts.Persistence;
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
                    return await roleRepository.ExistsAsync(id);
                })
                .WithMessage("{PropertyName} does not exist");
        }
    }
}
