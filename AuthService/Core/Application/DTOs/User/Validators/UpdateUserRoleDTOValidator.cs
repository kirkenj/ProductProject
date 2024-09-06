using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserRoleDTOValidator : AbstractValidator<UpdateUserRoleDTO>
    {
        public UpdateUserRoleDTOValidator(IRoleRepository roleRepository)
        {
            Include(new IRoleEditIingDtoValidator(roleRepository));
        }
    }
}
