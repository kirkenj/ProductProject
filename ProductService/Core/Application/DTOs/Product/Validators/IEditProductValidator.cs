using Application.DTOs.Product.Contracts;
using FluentValidation;


namespace Application.DTOs.Product.Validators
{
    public class IEditProductValidator : AbstractValidator<IEditProductDto>
    {
        public IEditProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEqual(default(decimal));
            RuleFor(x => x.CreationDate).NotEqual(default(DateTime));
            RuleFor(x => x.ProducerId).NotEqual(default(Guid));
        }
    }
}
