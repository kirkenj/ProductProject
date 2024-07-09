using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserDetailHandler : IRequestHandler<GetUserDtoRequest, UserDto>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserDetailHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserDtoRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.Id);
            return mapper.Map<UserDto>(user);
        }
    }
}
