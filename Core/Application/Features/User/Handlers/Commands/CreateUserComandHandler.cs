using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Exceptions;
using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;

namespace Application.Features.User.Handlers.Commands
{
    public class CreateUserComandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper; 
        private readonly IUserPasswordSetter passwordSetter;
        private readonly CreateUserSettings createUserSettings;


        public CreateUserComandHandler(IOptions<CreateUserSettings> createUserSettings, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IEmailSender emailSender, IUserPasswordSetter passwordSetter)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.passwordSetter = passwordSetter;
            this.createUserSettings = createUserSettings.Value;
        }


        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserDtoValidator(roleRepository, userRepository);

            var result = await validator.ValidateAsync(request.CreateUserDto, cancellationToken);

            if (result.IsValid == false)
            {
                if (result is null)
                {
                    throw new InvalidProgramException($"{nameof(result)} is null");
                }

                throw new ValidationException(result);
            }

            var user = mapper.Map<Domain.Models.User>(request.CreateUserDto);
            user.Id = Guid.NewGuid();
            user.RoleID = createUserSettings.DefaultRoleID;

            passwordSetter.SetPassword(user, request.CreateUserDto.Password);

            await userRepository.AddAsync(user);

            if (user.Email != null && emailSender != null)
            {
                var email = new Email { Body = "Email confirmed", Subject = "Email confirmation", To = user.Email };

                await emailSender.SendEmailAsync(email);
            }

            return user.Id;
        }
    }
}
