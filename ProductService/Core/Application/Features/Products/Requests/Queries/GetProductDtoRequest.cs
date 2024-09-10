using Application.DTOs.Product;
using CustomResponse;
using MediatR;

namespace Application.Features.Product.Requests.Queries
{
    public class GetProductDtoRequest : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
    }
}
