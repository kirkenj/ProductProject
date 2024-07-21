using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Options;
using Application.Models.User;
using Application.Models.Email;
using Application.Contracts.Application;
using Application.Models.Response;
using System.Net;

namespace Application.Features.User.Handlers.Commands
{
    public class CreateUserComandHandler : IRequestHandler<CreateUserCommand, Response<Guid>>, IPasswordSettingHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper; 
        private readonly CreateUserSettings createUserSettings;


        public CreateUserComandHandler(IOptions<CreateUserSettings> createUserSettings, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IHashProvider passwordSetter)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.HashPrvider = passwordSetter;
            this.createUserSettings = createUserSettings.Value;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserDtoValidator(userRepository);

            var validationResult = await validator.ValidateAsync(request.CreateUserDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<Guid>.BadRequestResponse(validationResult.Errors);
            }

            var user = mapper.Map<Domain.Models.User>(request.CreateUserDto);
            user.Id = Guid.NewGuid();
            user.RoleID = createUserSettings.DefaultRoleID;

            (this as IPasswordSettingHandler).SetPassword(request.CreateUserDto.NewPassword, user);

            await userRepository.AddAsync(user);

            return Response<Guid>.OkResponse(user.Id, $"Created user's id: {user.Id}");
        }
    }
}
