using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
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
            var validator = new UpdateUserRoleDTOValidator(_roleRepository);

            var validationResult = await validator.ValidateAsync(request.UpdateUserRoleDTO, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(string.Join("; ", validationResult.Errors));
            }

            Domain.Models.User? user = await _userRepository.GetAsync(request.UpdateUserRoleDTO.UserId);

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
