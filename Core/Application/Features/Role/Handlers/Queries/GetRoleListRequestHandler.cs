using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetRoleListRequestHandler : IRequestHandler<GetRoleListRequest, List<RoleDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetRoleListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }


        async Task<List<RoleDto>> IRequestHandler<GetRoleListRequest, List<RoleDto>>.Handle(GetRoleListRequest request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<List<RoleDto>>(users);
        }
    }
}
