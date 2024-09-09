using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Models.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserDetailHandler : IRequestHandler<GetUserDtoRequest, Response<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDetailHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<UserDto>> Handle(GetUserDtoRequest request, CancellationToken cancellationToken)
        {
            Domain.Models.User? user = await _userRepository.GetAsync(request.Id);

            return user == null ?
                Response<UserDto>.NotFoundResponse(nameof(request.Id), true)
                : Response<UserDto>.OkResponse(_mapper.Map<UserDto>(user), "Success");
        }
    }
}
