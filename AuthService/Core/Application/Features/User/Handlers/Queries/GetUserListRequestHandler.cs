using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Models.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserListRequestHandler : IRequestHandler<GetUserListRequest, Response<List<UserListDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<UserListDto>>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return Response<List<UserListDto>>.OkResponse(_mapper.Map<List<UserListDto>>(users), "Success");
        }
    }
}
