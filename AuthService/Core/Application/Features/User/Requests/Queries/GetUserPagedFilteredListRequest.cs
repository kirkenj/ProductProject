using Application.DTOs.User;
using Application.Models.Response;
using Application.Models.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class GetUserPagedFilteredListRequest : IRequest<Response<List<UserListDto>>>
    {
        public UserFilter UserFilter { get; set; } = null!;
        public int? PageSize { get; set; }
        public int? Page {  get; set; }
    }
}
