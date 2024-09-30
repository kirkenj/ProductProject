using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator(IRoleRepository roleRepository)
        {
            RuleFor(r => r.UpdateUserRoleDTO).NotNull().SetValidator(new UpdateUserRoleDTOValidator(roleRepository));
        }
    }
}
