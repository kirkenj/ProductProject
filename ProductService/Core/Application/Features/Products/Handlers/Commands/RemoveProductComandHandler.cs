using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.Product.Requests.Commands;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;

namespace Application.Features.Product.Handlers.Commands
{
    public class RemoveProductComandHandler : IRequestHandler<RemovePrductComand, Response<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAuthClientService _authClientService;

        public RemoveProductComandHandler(IProductRepository productRepository, IAuthClientService authClientService, IEmailSender emailSender)
        {
            _productRepository = productRepository;
            _authClientService = authClientService;
            _emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(RemovePrductComand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);

            if (product == null)
            {
                return Response<string>.NotFoundResponse(nameof(product.Id), true);
            }

            await _productRepository.DeleteAsync(product);

            var ownerResult = await _authClientService.GetUser(product.ProducerId) ?? throw new ApplicationException("Couldn't send request to auth sevice");

            if (ownerResult.Success && ownerResult.Result != null)
            {
                await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "Your product was removed",
                    To = ownerResult.Result.Email,
                    Body = $"Your product with id '{product.Id}' was removed"
                });
            }

            return Response<string>.OkResponse("Ok", "Success");
        }
    }
}
