using Application.Contracts.Persistence;
using Application.DTOs.Product.Validators;
using Application.Features.Product.Requests.Commands;
using AutoMapper;
using Clients.AuthApi;
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
        private readonly IAuthApiClient _authClientService;
        private readonly IEmailSender _emailSender;

        public CreateProductComandHandler(IProductRepository userRepository, IMapper mapper, IAuthApiClient authClientService, IEmailSender emailSender)
        {
            _productRepository = userRepository;
            _mapper = mapper;
            _authClientService = authClientService;
            _emailSender = emailSender;
        }

        public async Task<Response<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductDtoValidator(_authClientService);

            var validationResult = await validator.ValidateAsync(request.CreateProductDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<Guid>.BadRequestResponse(validationResult.ToString());
            }

            Domain.Models.Product product = _mapper.Map<Domain.Models.Product>(request.CreateProductDto);

            await _productRepository.AddAsync(product);

            UserDto userRequestResponse = await _authClientService.UsersGETAsync(request.CreateProductDto.ProducerId);

            await _emailSender.SendEmailAsync(new Email
            {
                Body = $"You added a product with id '{product.Id}'",
                Subject = "Product creation",
                To = userRequestResponse.Email
            });

            return Response<Guid>.OkResponse(product.Id, $"Product created with id {product.Id}");
        }
    }
}
