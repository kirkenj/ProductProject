using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserDtoRequest : IRequest<Response<UserDto>>
    {
        public Guid Id { get; set; }
    }
}
