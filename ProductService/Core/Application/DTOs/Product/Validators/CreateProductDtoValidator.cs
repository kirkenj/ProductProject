using Application.Contracts.Infrastructure;
using FluentValidation;

namespace Application.DTOs.Product.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator(IAuthClientService authClientService)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEqual(default(decimal));
            RuleFor(x => x.CreationDate).NotEqual(default(DateTime));
            RuleFor(x => x.ProducerId).NotEqual(default(Guid));
            RuleFor(x => x.ProducerId).MustAsync(async (id, token) =>
            {
                var result = await authClientService.GetUser(id);
                return result.Success && result.Result != null;
            });
        }
    }
}
