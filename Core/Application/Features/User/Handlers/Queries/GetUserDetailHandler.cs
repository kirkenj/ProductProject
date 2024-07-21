using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserDetailHandler : IRequestHandler<GetUserDtoRequest, Response<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserDetailHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Response<UserDto>> Handle(GetUserDtoRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.Id, true);

            if (user == null)
            { 
                return Response<UserDto>.NotFoundResponse(nameof(request.Id), true);
            }

            return Response<UserDto>.OkResponse(mapper.Map<UserDto>(user), "Success");
        }
    }
}
