using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserListRequestHandler : IRequestHandler<GetUserListRequest, Response<List<UserListDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Response<List<UserListDto>>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync();
            return Response<List<UserListDto>>.OkResponse(mapper.Map<List<UserListDto>>(users), "Success");
        }
    }
}
