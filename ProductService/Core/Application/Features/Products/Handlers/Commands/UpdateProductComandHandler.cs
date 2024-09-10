using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.UserClient;
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
        private readonly IAuthClientService _authClientService;
        private readonly IEmailSender _emailSender;

        public UpdateProductComandHandler(IProductRepository productRepository, IMapper mapper, IAuthClientService authClientService, IEmailSender emailSender)
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
                Response<string> updateProduserResult = await UpdateProducer(product, newOwnerId);
                if (updateProduserResult.Success == false)
                {
                    return updateProduserResult;
                }
            }

            _mapper.Map(request.UpdateProductDto, product);
            await _productRepository.UpdateAsync(product);
            return Response<string>.OkResponse("Success", "Product updated");
        }

        private async Task<Response<string>> UpdateProducer(Domain.Models.Product product, Guid newProducerId)
        {
            var prevOwnedId = product.ProducerId;

            ClientResponse<ICollection<UserListDto>>? usersRequest = await _authClientService.ListAsync(ids: new Guid[] { prevOwnedId, newProducerId });

            if (usersRequest == null || usersRequest.Success == false)
            {
                throw new ApplicationException("Couldn't get users from auth service. " + usersRequest?.Message ?? string.Empty);
            }

            var newOwner = usersRequest.Result.FirstOrDefault(u => u.Id == newProducerId);

            if (newOwner == null)
            {
                return Response<string>.BadRequestResponse($"Couldn't find user with id = '{newProducerId}'");
            }

            await _emailSender.SendEmailAsync(new Email
            {
                Subject = "You were given a product",
                To = newOwner.Email,
                Body = $"Your new product id is {product.Id}"
            });

            var prevOwner = usersRequest.Result.FirstOrDefault(u => u.Id == prevOwnedId);
            if (prevOwner != null && prevOwner.Email != null)
            {
                await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "Your product was given to other user",
                    To = prevOwner.Email,
                    Body = $"Your product with id '{product.Id}' was given to other user"
                });
            }

            return Response<string>.OkResponse("Success", "Product updated");
        }
    }
}
