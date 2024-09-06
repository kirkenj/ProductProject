using Application.DTOs.Product;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Product.Requests.Queries
{
    public class GetProductDtoRequest : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
    }
}
