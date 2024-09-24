using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Response<string>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public UpdateUserRoleCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.User? user = await _userRepository.GetAsync(request.UpdateUserRoleDTO.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Email), true);
            }

            user.RoleID = request.UpdateUserRoleDTO.RoleID;

            await _userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Role updated");
        }
    }
}
