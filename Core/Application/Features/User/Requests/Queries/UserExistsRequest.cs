using Domain.Common.Interfaces;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class UserExistsRequest : IIdObject<Guid>, IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
