using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Commands.Validators
{
    public class UpdateNotSensitiveUserInfoComandValidator : AbstractValidator<UpdateNotSensitiveUserInfoComand>
    {
        public UpdateNotSensitiveUserInfoComandValidator()
        {
            RuleFor(r => r.UpdateUserInfoDto).SetValidator(new UpdateUserInfoDtoValidator());
        }
    }
}
