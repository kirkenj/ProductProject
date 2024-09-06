using Application.DTOs.Product;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Product.Requests.Commands
{
    public class UpdateProductCommand : IRequest<Response<string>>
    {
        public UpdateProductDto UpdateProductDto { get; set; } = null!;
    }
}
