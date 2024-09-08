using Application.Models.Response;
using MediatR;
using Repository.Contracts;

namespace Application.Features.User.Requests.Queries
{
    public class UserExistsRequest : IIdObject<Guid>, IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }
}
