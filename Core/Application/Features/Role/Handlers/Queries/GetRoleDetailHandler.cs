using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetRoleDetailHandler : IRequestHandler<GetRoleDtoRequest, RoleDto?>
    {
        private readonly IRoleRepository userRepository;
        private readonly IMapper mapper;

        public GetRoleDetailHandler(IRoleRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<RoleDto?> Handle(GetRoleDtoRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.Id);
            return user == null ? null : mapper.Map<RoleDto>(user);
        }
    }
}
