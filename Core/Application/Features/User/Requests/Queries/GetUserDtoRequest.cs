using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserDtoRequest : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
