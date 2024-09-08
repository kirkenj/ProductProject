using Application.Contracts.Persistence;
using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using Application.Models.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetRoleListRequestHandler : IRequestHandler<GetRoleListRequest, Response<List<RoleDto>>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetRoleListRequestHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<RoleDto>>> Handle(GetRoleListRequest request, CancellationToken cancellationToken)
        {
            var users = await _roleRepository.GetAllAsync();
            return Response<List<RoleDto>>.OkResponse(_mapper.Map<List<RoleDto>>(users), "Success");
        }
    }
}
