using Application.DTOs.User;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserDtoRequest : IRequest<Response<UserDto>>
    {
        public Guid Id { get; set; }
    }
}
