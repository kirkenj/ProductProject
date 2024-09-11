using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserInfoDtoValidator : AbstractValidator<UpdateUserInfoDto>
    {
        public UpdateUserInfoDtoValidator()
        {
            Include(new IIdDtoValidator<Guid>());
            Include(new IUserInfoDtoValidator());
        }
    }
}
