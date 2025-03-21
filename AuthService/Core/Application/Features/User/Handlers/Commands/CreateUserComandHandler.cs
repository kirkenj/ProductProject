﻿using Application.Contracts.Application;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using Application.Models.User;
using AutoMapper;
using Cache.Contracts;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using HashProvider.Contracts;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.User.Handlers.Commands
{
    public class CreateUserComandHandler : IRequestHandler<CreateUserCommand, Response<Guid>>, IPasswordSettingHandler
    {
        private readonly CreateUserSettings _createUserSettings;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public CreateUserComandHandler(IOptions<CreateUserSettings> createUserSettings, IMapper mapper, IHashProvider passwordSetter, IPasswordGenerator passwordGenerator, IEmailSender emailSender, ICustomMemoryCache memoryCache, IRoleRepository roleRepository)
        {
            _createUserSettings = createUserSettings.Value;
            _passwordGenerator = passwordGenerator;
            _roleRepository = roleRepository;
            HashPrvider = passwordSetter;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.User user = _mapper.Map<Domain.Models.User>(request.CreateUserDto);

            user.Id = Guid.NewGuid();
            user.Login = $"User{user.Id}";
            user.RoleID = _createUserSettings.DefaultRoleID;

            string password = _passwordGenerator.Generate();

            (this as IPasswordSettingHandler).SetPassword(password, user);

            await _memoryCache.SetAsync(string.Format(_createUserSettings.KeyForRegistrationCachingFormat, user.Email), user, TimeSpan.FromHours(_createUserSettings.EmailConfirmationTimeoutHours));

            bool isEmailSent = await _emailSender.SendEmailAsync(new Email
            {
                Subject = "Registration",
                Body = string.Format(_createUserSettings.BodyMessageFormat, user.Email, password),
                To = user.Email
            });

            if (isEmailSent == false)
            {
                throw new ApplicationException("User request created, but email was not sent");
            }

            return Response<Guid>.OkResponse(user.Id, $"Created user registration request. Further details sent on email");
        }
    }
}
