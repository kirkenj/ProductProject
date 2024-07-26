using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserLoginDtoValidator : AbstractValidator<UpdateUserLoginDto>
    {
        public UpdateUserLoginDtoValidator()
        {
            RuleFor(o => o.Id).NotEqual(Guid.Empty).WithMessage("{PropertyName} can not be equal to {ComparisonValue}");
        }
    }
}
