using Application.DTOs.User.Validators;
using Application.Exceptions;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Unit>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;

        public UpdateUserRoleCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserRoleDTOValidator(roleRepository);

            var result = await validator.ValidateAsync(request.UpdateUserRoleDTO, cancellationToken);

            if (result.IsValid == false)
            {
                throw new ValidationException(result);
            }

            var user = await userRepository.GetAsync(request.UpdateUserRoleDTO.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), $"{nameof(request.UpdateUserRoleDTO.UserId)} = {request.UpdateUserRoleDTO.RoleID}");
            }

            user.RoleID = request.UpdateUserRoleDTO.RoleID;
            
            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
