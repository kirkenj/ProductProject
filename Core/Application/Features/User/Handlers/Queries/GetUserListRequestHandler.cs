using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserListRequestHandler : IRequestHandler<GetUserListRequest, List<UserListDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }


        async Task<List<UserListDto>> IRequestHandler<GetUserListRequest, List<UserListDto>>.Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<List<UserListDto>>(users);
        }
    }
}
