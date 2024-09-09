using Application.Contracts.Persistence;
using Application.Features.User.Requests.Queries;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class UserExistsRequestHandler : IRequestHandler<UserExistsRequest, Response<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UserExistsRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<bool>> Handle(UserExistsRequest request, CancellationToken cancellationToken) 
        {
            bool userExcists = await _userRepository.GetAsync(request.Id) is null;
            return Response<bool>.OkResponse(userExcists, string.Empty); 
        }
    }
}
