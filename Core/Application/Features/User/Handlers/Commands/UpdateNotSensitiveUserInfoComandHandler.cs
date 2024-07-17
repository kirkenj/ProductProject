using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateNotSensitiveUserInfoComandHandler : IRequestHandler<UpdateNotSensitiveUserInfoComand, Response<string>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UpdateNotSensitiveUserInfoComandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Response<string>> Handle(UpdateNotSensitiveUserInfoComand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.UpdateUserAddressDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            mapper.Map(request.UpdateUserAddressDto, user);

            await userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Success");
        }
    }
}
