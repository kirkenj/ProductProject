using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using Application.Models.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateNotSensitiveUserInfoComandHandler : IRequestHandler<UpdateNotSensitiveUserInfoComand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateNotSensitiveUserInfoComandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(UpdateNotSensitiveUserInfoComand request, CancellationToken cancellationToken)
        {
            Domain.Models.User? user = await _userRepository.GetAsync(request.UpdateUserAddressDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(request.UpdateUserAddressDto.Id), true);
            }

            _mapper.Map(request.UpdateUserAddressDto, user);

            await _userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Success");
        }
    }
}
