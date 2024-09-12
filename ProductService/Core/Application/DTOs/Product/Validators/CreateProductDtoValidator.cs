using Application.Contracts.AuthService;
using FluentValidation;

namespace Application.DTOs.Product.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator(IAuthApiClientService authClientService)
        {
            Include(new IEditProductValidator());
            RuleFor(x => x.ProducerId).MustAsync(async (id, token) =>
            {
                var result = await authClientService.GetUser(id);
                return result.Success;
            });
        }
    }
}
