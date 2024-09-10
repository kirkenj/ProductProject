using CustomResponse;
using MediatR;

namespace Application.Features.Product.Requests.Commands
{
    public class RemovePrductComand : IRequest<Response<string>>
    {
        public Guid ProductId { get; set; }
    }
}
