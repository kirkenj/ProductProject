using Application.DTOs.User;
using CustomResponse;
using Application.Models.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserListRequest : IRequest<Response<List<UserListDto>>>
    {
        public UserFilter UserFilter { get; set; } = null!;
        public int? PageSize { get; set; }
        public int? Page { get; set; }
    }
}
