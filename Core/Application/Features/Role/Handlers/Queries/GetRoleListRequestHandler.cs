using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Features.User.Handlers.Queries
{
    public class GetRoleListRequestHandler : IRequestHandler<GetRoleListRequest, Response<List<RoleDto>>>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public GetRoleListRequestHandler(IRoleRepository userRepository, IMapper mapper)
        {
            this.roleRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Response<List<RoleDto>>> Handle(GetRoleListRequest request, CancellationToken cancellationToken)
        {
            var users = await roleRepository.GetAllAsync();
            return Response<List<RoleDto>>.OkResponse(mapper.Map<List<RoleDto>>(users), "Success");
        }
    }
}
