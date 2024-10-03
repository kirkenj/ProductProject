using Application.Contracts.AuthService;
using Application.Contracts.Persistence;
using Application.Features.Product.Requests.Commands;
using AutoMapper;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;


namespace Application.Features.Product.Handlers.Commands
{
    public class CreateProductComandHandler : IRequestHandler<CreateProductCommand, Response<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IAuthApiClientService _authClientService;
        private readonly IEmailSender _emailSender;

        public CreateProductComandHandler(IProductRepository userRepository, IMapper mapper, IAuthApiClientService authClientService, IEmailSender emailSender)
        {
            _productRepository = userRepository;
            _mapper = mapper;
            _authClientService = authClientService;
            _emailSender = emailSender;
        }

        public async Task<Response<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.Product product = _mapper.Map<Domain.Models.Product>(request.CreateProductDto);

            var addTask = _productRepository.AddAsync(product);

            var msgTask = Task.Run(async () =>
            {
                var producerId = request.CreateProductDto.ProducerId;

                var userResponse = await _authClientService.GetUser(producerId);

                var user = userResponse.Result ?? throw new ApplicationException($"Couldn't get user with id '{producerId}'");

                if (user.Email != null)
                {
                    await _emailSender.SendEmailAsync(new Email
                    {
                        Body = $"You added a product with id '{product.Id}'",
                        Subject = "Product creation",
                        To = user.Email
                    });
                }
            }, cancellationToken);

            await Task.WhenAll(msgTask, msgTask);

            return Response<Guid>.OkResponse(product.Id, $"Product created with id {product.Id}");
        }
    }
}
