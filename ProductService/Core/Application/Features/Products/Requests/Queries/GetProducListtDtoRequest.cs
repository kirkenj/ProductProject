using Application.DTOs.Product;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Product.Requests.Queries
{
    public class GetProducListtDtoRequest : IRequest<Response<IEnumerable<ProductListDto>>>
    {
        public int? Page {  get; set; }
        public int? PageSize {  get; set; }
    }
}
