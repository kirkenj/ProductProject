using Application.Contracts.Persistence;
using Application.DTOs.Product;
using Application.Features.Product.Requests.Queries;
using AutoMapper;
using CustomResponse;
using MediatR;

namespace Application.Features.Product.Handlers.Queries
{
    public class GetProductDetailHandler : IRequestHandler<GetProductDtoRequest, Response<ProductDto>>
    {
        private readonly IProductRepository _producrRepository;
        private readonly IMapper _mapper;

        public GetProductDetailHandler(IProductRepository userRepository, IMapper mapper)
        {
            _producrRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<ProductDto>> Handle(GetProductDtoRequest request, CancellationToken cancellationToken)
        {
            var user = await _producrRepository.GetAsync(request.Id);

            if (user == null)
            {
                return Response<ProductDto>.NotFoundResponse(nameof(request.Id), true);
            }

            return Response<ProductDto>.OkResponse(_mapper.Map<ProductDto>(user), "Success");
        }
    }
}
