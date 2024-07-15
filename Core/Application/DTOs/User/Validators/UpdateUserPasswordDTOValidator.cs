using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserPasswordDTOValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDTOValidator()
        {
            Include(new IUpdateUserPasswordDtoValidator());
            RuleFor(o => o.NewPassword).NotEqual(o => o.OldPassword).WithMessage("New password can not be same as the old one");
            RuleFor(o => o.Id).NotEqual(Guid.Empty).WithMessage("{PropertyName} can not be equal to {ComparisonValue}");
        }
    }
}
