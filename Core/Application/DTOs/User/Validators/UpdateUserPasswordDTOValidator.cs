using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserPasswordDTOValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDTOValidator()
        {
            Include(new IUpdateUserPasswordDtoValidator());
            RuleFor(o => o.Id).NotEqual(Guid.Empty).WithMessage("{PropertyName} can not be equal to {ComparisonValue}");
        }
    }
}
