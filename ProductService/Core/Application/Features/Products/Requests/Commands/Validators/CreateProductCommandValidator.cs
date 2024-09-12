using Application.Contracts.AuthService;
using Application.DTOs.Product.Validators;
using Application.Features.Product.Requests.Commands;
using FluentValidation;

namespace Application.Features.Products.Requests.Commands.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IAuthApiClientService authApiClient)
        {
            RuleFor(o => o.CreateProductDto).SetValidator(new CreateProductDtoValidator(authApiClient));
        }
    }
}
