using Application.Features.Product.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Response;
using Application.Contracts.Infrastructure;
using EmailSender.Contracts;
using EmailSender.Models;

namespace Application.Features.Product.Handlers.Commands
{
    public class RemoveProductComandHandler : IRequestHandler<RemovePrductComand, Response<string>>
    {
        private readonly IProductRepository productRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAuthClientService _authClientService;

        public RemoveProductComandHandler(IProductRepository productRepository, IAuthClientService authClientService, IEmailSender emailSender)
        {
            this.productRepository = productRepository;
            _authClientService = authClientService;
            _emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(RemovePrductComand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetAsync(request.ProductId);

            if (product == null)
            {
                return Response<string>.NotFoundResponse(nameof(product.Id), true);
            }

            await productRepository.DeleteAsync(product);

            var ownerResult = await _authClientService.GetUser(product.ProducerId) ?? throw new ApplicationException();

            if (ownerResult != null && ownerResult.Success && ownerResult.Result != null)
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
