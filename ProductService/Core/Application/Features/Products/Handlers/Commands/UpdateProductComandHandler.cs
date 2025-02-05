﻿using Application.Contracts.AuthService;
using Application.Contracts.Persistence;
using Application.Features.Product.Requests.Commands;
using AutoMapper;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;

namespace Application.Features.Product.Handlers.Commands
{
    public class UpdateProductComandHandler : IRequestHandler<UpdateProductCommand, Response<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IAuthApiClientService _authClientService;
        private readonly IEmailSender _emailSender;

        public UpdateProductComandHandler(IProductRepository productRepository, IMapper mapper, IAuthApiClientService authClientService, IEmailSender emailSender)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _authClientService = authClientService;
            _emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.UpdateProductDto.Id);

            if (product == null)
            {
                return Response<string>.NotFoundResponse(nameof(product.Id), true);
            }

            var newOwnerId = request.UpdateProductDto.ProducerId;

            if (product.ProducerId != newOwnerId)
            {
                await NotifyProducers(product, newOwnerId);
            }

            _mapper.Map(request.UpdateProductDto, product);
            await _productRepository.UpdateAsync(product);
            return Response<string>.OkResponse("Success", "Product updated");
        }

        private async Task NotifyProducers(Domain.Models.Product product, Guid newProducerId)
        {
            var prevOwnedId = product.ProducerId;

            var newOwnerResponse = await _authClientService.GetUser(newProducerId);

            if (!newOwnerResponse.Success)
            {
                throw new ApplicationException($"Couldn't find user with id = '{newProducerId}'");
            }


            var newOwnerEmail = newOwnerResponse.Result?.Email ?? null;
            if (string.IsNullOrEmpty(newOwnerEmail) == false)
            {
                await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "You were given a product",
                    To = newOwnerEmail,
                    Body = $"Your new product id is {product.Id}"
                });
            }

            var prevOwner = await _authClientService.GetUser(prevOwnedId);
            var prevOwnerEmail = prevOwner.Result?.Email ?? null;
            if (string.IsNullOrEmpty(prevOwnerEmail) == false)
            {
                await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "Your product was given to other user",
                    To = prevOwnerEmail,
                    Body = $"Your product with id '{product.Id}' was given to other user"
                });
            }
        }
    }
}
