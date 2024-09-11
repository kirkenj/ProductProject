using Application.Contracts.Persistence;
using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserRoleDTOValidator : AbstractValidator<UpdateUserRoleDTO>
    {
        public UpdateUserRoleDTOValidator(IRoleRepository roleRepository)
        {
            Include(new IIdDtoValidator<Guid>());
            Include(new IRoleDtoValidator(roleRepository));
        }
    }
}
