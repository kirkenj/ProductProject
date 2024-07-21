using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class UserExistsRequestHandler : IRequestHandler<UserExistsRequest, Response<bool>>
    {
        private readonly IUserRepository userRepository;

        public UserExistsRequestHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Response<bool>> Handle(UserExistsRequest request, CancellationToken cancellationToken) 
            => Response<bool>.OkResponse(await userRepository.GetAsync(request.Id) is null, string.Empty);
    }
}
