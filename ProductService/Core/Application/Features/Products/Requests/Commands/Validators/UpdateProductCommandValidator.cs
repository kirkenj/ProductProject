using Application.Contracts.AuthService;
using Application.Contracts.Persistence;
using Application.DTOs.Product.Validators;
using Application.Features.Product.Requests.Commands;
using FluentValidation;

namespace Application.Features.Products.Requests.Commands.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IAuthApiClientService authApiClientService, IProductRepository productRepository)
        {
            RuleFor(u => u.UpdateProductDto).SetValidator(new UpdateProductDtoValidator(authApiClientService, productRepository));
        }
    }
}
