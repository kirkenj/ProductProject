using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using AutoMapper;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserListHandler : IRequestHandler<GetUserListRequest, Response<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserListHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<UserDto>>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Domain.Models.User> users = await _userRepository.GetPageContent(request.UserFilter, request.Page, request.PageSize);
            return Response<List<UserDto>>.OkResponse(_mapper.Map<List<UserDto>>(users), "Success");
        }
    }
}
