using Application.Contracts.Persistence;
using Application.Features.Product.Requests.Commands;
using AutoMapper;
using Clients.AuthApi;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;
using UserDto = Clients.AuthApi.UserDto;

namespace Application.Features.Product.Handlers.Commands
{
    public class RemoveProductComandHandler : IRequestHandler<RemovePrductComand, Response<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAuthApiClient _authClientService;
        private readonly IMapper _mapper;

        public RemoveProductComandHandler(IProductRepository productRepository, IAuthApiClient authClientService, IEmailSender emailSender, IMapper mapper)
        {
            _productRepository = productRepository;
            _authClientService = authClientService;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RemovePrductComand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);

            if (product == null)
            {
                return Response<string>.NotFoundResponse(nameof(product.Id), true);
            }

            await _productRepository.DeleteAsync(product.Id);

            UserDto ownerResult = _mapper.Map<UserDto>(await _authClientService.UsersGETAsync(product.ProducerId, cancellationToken));

            if (ownerResult != null && ownerResult.Email != null)
            {
                await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "Your product was removed",
                    To = ownerResult.Email,
                    Body = $"Your product with id '{product.Id}' was removed"
                });
            }

            return Response<string>.OkResponse("Ok", "Success");
        }
    }
}
