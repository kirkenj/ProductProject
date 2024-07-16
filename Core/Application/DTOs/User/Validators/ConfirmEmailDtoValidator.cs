using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User.Validators
{
    internal class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {
            RuleFor(o => o.UserId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(o => o.Key).NotNull().NotEmpty();
        }
    }
}
