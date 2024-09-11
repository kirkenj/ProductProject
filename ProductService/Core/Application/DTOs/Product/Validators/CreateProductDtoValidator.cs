using Clients.AuthApi;
using FluentValidation;

namespace Application.DTOs.Product.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator(IAuthApiClient authClientService)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEqual(default(decimal));
            RuleFor(x => x.CreationDate).NotEqual(default(DateTime));
            RuleFor(x => x.ProducerId).NotEqual(default(Guid));
            RuleFor(x => x.ProducerId).MustAsync(async (id, token) =>
            {
                var result = await authClientService.UsersGETAsync(id);
                return result != null;
            });
        }
    }
}
