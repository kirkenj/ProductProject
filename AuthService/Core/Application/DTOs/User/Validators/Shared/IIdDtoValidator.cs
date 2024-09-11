using FluentValidation;
using Repository.Contracts;

namespace Application.DTOs.User.Validators.Shared
{
    public class IIdDtoValidator<T> : AbstractValidator<IIdObject<T>> where T : struct
    {
        public IIdDtoValidator()
        {
            RuleFor(p => p.Id).NotEqual(default(T));
        }
    }
}
