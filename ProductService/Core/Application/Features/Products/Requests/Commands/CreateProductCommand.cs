using Application.DTOs.Product;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Product.Requests.Commands
{
    public class CreateProductCommand : IRequest<Response<Guid>>
    {
        public CreateProductDto CreateProductDto { get; set; } = null!;
    }
}
