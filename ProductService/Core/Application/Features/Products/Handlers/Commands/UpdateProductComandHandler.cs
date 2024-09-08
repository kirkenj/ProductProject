using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.Product.Requests.Commands;
using Application.Models.Response;
using AutoMapper;
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

            var prevOwnedId = product.ProducerId;
            var newOwnerId = request.UpdateProductDto.ProducerId;

            if (prevOwnedId != newOwnerId)
            {
                var usersRequest = await _authClientService.ListAsync(ids: new Guid[] { prevOwnedId, newOwnerId });

                if (usersRequest == null || usersRequest.Success == false)
                {
                    throw new ApplicationException("Couldn't get users from auth service. " + usersRequest?.Message ?? string.Empty);
                }

                var newOwnerResult = usersRequest.Result.FirstOrDefault(u => u.Id == newOwnerId);

                if (newOwnerResult == null)
                {
                    return Response<string>.BadRequestResponse($"Couldn't find user with id = '{newOwnerId}'");
                }

                _ = Task.Run(async () => await _emailSender.SendEmailAsync(new Email
                {
                    Subject = "You were given a product",
                    To = newOwnerResult.Email,
                    Body = $"Your new product id is {request.UpdateProductDto.Id}"
                }));

                var prevOwnerResult = usersRequest.Result.FirstOrDefault(u => u.Id == prevOwnedId);
                if (prevOwnerResult != null && prevOwnerResult.Email != null)
                {
                    _ = Task.Run(async () => await _emailSender.SendEmailAsync(new Email
                    {
                        Subject = "Your product was given to other user",
                        To = prevOwnerResult.Email,
                        Body = $"Your product with id '{product.Id}' was given to other user"
                    }));
                }
            }

            _mapper.Map(request.UpdateProductDto, product);
            await _productRepository.UpdateAsync(product);
            return Response<string>.OkResponse("Ok", "Success");
        }
    }
}
