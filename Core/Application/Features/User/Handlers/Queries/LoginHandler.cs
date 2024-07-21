﻿using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Contracts.Infrastructure;
using System.Text;
using Application.Models.Email;
using Application.Models.User;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Queries
{
    public class LoginHandler : IRequestHandler<LoginDto, Response<string?>>
    {
        private readonly IUserRepository userRepository;
        private readonly IHashProvider hashProvider;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IJwtService jwtService;

        public LoginHandler(IUserRepository userRepository, IHashProvider hashProvider, IMapper mapper, IEmailSender emailSender, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.hashProvider = hashProvider;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.jwtService = jwtService;
        }

        public async Task<Response<string?>> Handle(LoginDto request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(new UserFilter { AccurateLogin = request.Login }, true);

            if (user == null)
            {
                return Response<string?>.NotFoundResponse(request.Login, true);
            }

            hashProvider.HashAlgorithmName = user.HashAlgorithm;
            hashProvider.Encoding = Encoding.GetEncoding(user.StringEncoding);

            string hash = hashProvider.GetHash(request.Password);

            if (hash == user.PasswordHash) 
            {
                if (user.IsEmailConfirmed && user.Email != null)
                {
                    await emailSender.SendEmailAsync(new Email { Body = "Someone logged in your acc", Subject = "New login", To = user.Email });
                }

                var mapped = mapper.Map<UserDto>(user);
                return Response<string?>.OkResponse(jwtService.GetToken(mapped), "Success");
            }

            return Response<string?>.BadRequestResponse("Wrong password");
        }
    }
}
