using Application.Contracts.Persistence;
using Application.DTOs.Product;
using Application.Features.Product.Requests.Queries;
using CustomResponse;
using AutoMapper;
using MediatR;

namespace Application.Features.Product.Handlers.Queries
{
    public class GetProductListHandler : IRequestHandler<GetProducListtDtoRequest, Response<IEnumerable<ProductListDto>>>
    {
        private readonly IProductRepository _producrRepository;
        private readonly IMapper _mapper;

        public GetProductListHandler(IProductRepository productRepository, IMapper mapper)
        {
            _producrRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProductListDto>>> Handle(GetProducListtDtoRequest request, CancellationToken cancellationToken)
        {
            var result = await _producrRepository.GetPageContent(request.Page, request.PageSize);

            return Response<IEnumerable<ProductListDto>>.OkResponse(_mapper.Map<List<ProductListDto>>(result), "Success");
        }
    }
}
