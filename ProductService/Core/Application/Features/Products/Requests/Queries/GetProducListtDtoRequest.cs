using Application.DTOs.Product;
using Application.Models.Product;
using CustomResponse;
using MediatR;

namespace Application.Features.Product.Requests.Queries
{
    public class GetProducListtDtoRequest : IRequest<Response<IEnumerable<ProductListDto>>>
    {
        public ProductFilter ProductFilter { get; set; } = null!;
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
