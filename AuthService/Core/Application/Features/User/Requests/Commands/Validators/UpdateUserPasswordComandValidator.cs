using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class UpdateUserPasswordComandValidator : AbstractValidator<UpdateUserPasswordComand>
    {
        public UpdateUserPasswordComandValidator()
        {
            RuleFor(r => r.UpdateUserPasswordDto).SetValidator(new UpdateUserPasswordDTOValidator());
        }
    }
}
