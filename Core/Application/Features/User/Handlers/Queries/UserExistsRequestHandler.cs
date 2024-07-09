using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class UserExistsRequestHandler : IRequestHandler<UserExistsRequest, bool>
    {
        private readonly IUserRepository userRepository;

        public UserExistsRequestHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserExistsRequest request, CancellationToken cancellationToken) => await userRepository.GetAsync(request.Id) is null;
    }
}
