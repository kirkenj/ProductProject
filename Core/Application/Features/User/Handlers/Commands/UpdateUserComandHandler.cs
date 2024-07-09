using Application.Exceptions;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserComandHandler : IRequestHandler<UpdateUserAddressComand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UpdateUserComandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserAddressComand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.UpdateUserAddressDto.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), $"{nameof(request.UpdateUserAddressDto.Address)} = {request.UpdateUserAddressDto.Address}");
            }

            mapper.Map(request.UpdateUserAddressDto, user);

            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
