using Application.DTOs.Role;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class GetRoleDetailHandler : IRequestHandler<GetRoleDtoRequest, Response<RoleDto>>
    {
        private readonly IRoleRepository _userRepository;
        private readonly IMapper _mapper;

        public GetRoleDetailHandler(IRoleRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<RoleDto>> Handle(GetRoleDtoRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id);
            return user == null ? 
                Response<RoleDto>.NotFoundResponse(nameof(request.Id), true) 
                : Response<RoleDto>.OkResponse(_mapper.Map<RoleDto>(user), string.Empty);
        }
    }
}
