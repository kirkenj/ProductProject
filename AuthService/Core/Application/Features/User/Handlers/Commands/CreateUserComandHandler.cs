using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Options;
using Application.Models.User;
using Application.Contracts.Application;
using Application.Models.Response;
using Cache.Contracts;
using EmailSender.Contracts;
using EmailSender.Models;

namespace Application.Features.User.Handlers.Commands
{
    public class CreateUserComandHandler : IRequestHandler<CreateUserCommand, Response<Guid>>, IPasswordSettingHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; 
        private readonly CreateUserSettings _createUserSettings;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IEmailSender _emailSender;

        public CreateUserComandHandler(IOptions<CreateUserSettings> createUserSettings, IUserRepository userRepository, IMapper mapper, IHashProvider passwordSetter, IPasswordGenerator passwordGenerator, IEmailSender emailSender, ICustomMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            HashPrvider = passwordSetter;
            _createUserSettings = createUserSettings.Value;
            _memoryCache = memoryCache;
            _passwordGenerator = passwordGenerator;
            _emailSender = emailSender;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserDtoValidator(_userRepository);

            var validationResult = await validator.ValidateAsync(request.CreateUserDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<Guid>.BadRequestResponse(validationResult.Errors);
            }

            var user = _mapper.Map<Domain.Models.User>(request.CreateUserDto);
            user.Id = Guid.NewGuid();
            user.Login = $"User{user.Id}";
            user.RoleID = _createUserSettings.DefaultRoleID;

            var password = _passwordGenerator.Generate();

            (this as IPasswordSettingHandler).SetPassword(password, user);

            _memoryCache.Set(CacheKeyGenerator.CacheKeyGenerator.KeyForRegistrationCaching(user.Email), user, DateTimeOffset.UtcNow.AddHours(1));

            var emailResult = await _emailSender.SendEmailAsync(new Email
            {
                Subject = "Registration",
                Body = $"Confirm registration by loggining in with email: {user.Email}, password {password}",
                To = user.Email
            });

            if (emailResult == true)
            {
                return Response<Guid>.OkResponse(user.Id, $"Created user's id: {user.Id}. Further details sent on email");
            }

            throw new ApplicationException("User created, but email was not sent");
        }
    }
}
