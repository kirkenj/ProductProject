using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserListRequest : IRequest<Response<List<UserListDto>>>
    {

    }
}
