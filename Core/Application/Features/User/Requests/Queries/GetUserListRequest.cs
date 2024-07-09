using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserListRequest : IRequest<List<UserListDto>>
    {

    }
}
