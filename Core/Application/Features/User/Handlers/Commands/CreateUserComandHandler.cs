using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Exceptions;
using Application.Contracts.Infrastructure;
using Application.Models;

namespace Application.Features.User.Handlers.Commands
{
    public class CreateUserComandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;
        private readonly IUserPasswordSetter userPasswordSetter;

        public CreateUserComandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IEmailSender emailSender, IUserPasswordSetter updateUserPasswordSetter)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.userPasswordSetter = updateUserPasswordSetter;
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

            var user = mapper.Map<Domain.Models.User>(request);
            user.Id = Guid.NewGuid();

            userPasswordSetter.SetPassword(user, request.CreateUserDto.Password);

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
