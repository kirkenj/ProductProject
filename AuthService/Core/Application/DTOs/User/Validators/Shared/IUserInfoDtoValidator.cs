using Application.Contracts.Persistence;
using Application.DTOs.User.Interfaces;
using FluentValidation;

namespace Application.DTOs.User.Validators.Shared
{
    public class IUserInfoDtoValidator : AbstractValidator<IUserInfoDto>
    {
        public IUserInfoDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Address).NotEmpty();
        }
    }
}
