using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Response<string>>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;

        public UpdateUserRoleCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public async Task<Response<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserRoleDTOValidator(roleRepository);

            var validationResult = await validator.ValidateAsync(request.UpdateUserRoleDTO, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(validationResult.Errors);
            }

            var user = await userRepository.GetAsync(request.UpdateUserRoleDTO.UserId);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Email), true);
            }

            user.RoleID = request.UpdateUserRoleDTO.RoleID;
            
            await userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Role updated");
        }
    }
}
