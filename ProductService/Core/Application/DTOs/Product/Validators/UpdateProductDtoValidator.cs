using FluentValidation;

namespace Application.DTOs.Product.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEqual(default(decimal));
            RuleFor(x => x.CreationDate).NotEqual(default(DateTime));
            RuleFor(x => x.ProducerId).NotEqual(default(Guid));
            RuleFor(x => x.Id).NotEqual(default(Guid));
        }
    }
}
