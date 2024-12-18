﻿using Application.Contracts.AuthService;
using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.DTOs.Product.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator(IAuthApiClientService authApiClientService, IProductRepository productRepository)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            Include(new IEditProductValidator());
            RuleFor(x => x.Id).NotEqual(default(Guid)).MustAsync(async (id, token) =>
            {
                var user = await productRepository.GetAsync(id);
                return null != user;
            });

            RuleFor(x => x).MustAsync(async (updateProductDto, cancellationToken) =>
            {
                var currentProductState = await productRepository.GetAsync(updateProductDto.Id) ?? throw new Exception();

                if (updateProductDto.ProducerId != currentProductState.ProducerId)
                {
                    var response = await authApiClientService.GetUser(updateProductDto.ProducerId);
                    return response.Success;
                }

                return true;
            }).WithMessage((x) => $"User with id '{x.ProducerId}' does not exist");
        }
    }
}
