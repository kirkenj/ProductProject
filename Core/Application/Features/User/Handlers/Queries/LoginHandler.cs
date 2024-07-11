using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Contracts.Infrastructure;
using System.Text;

namespace Application.Features.User.Handlers.Queries
{
    public class LoginHandler : IRequestHandler<LoginDto, UserDto?>
    {
        private readonly IUserRepository userRepository;
        private readonly IHashProvider hashProvider;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

        public LoginHandler(IUserRepository userRepository, IHashProvider hashProvider, IMapper mapper, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.hashProvider = hashProvider;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }

        public async Task<UserDto?> Handle(LoginDto request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(new Models.UserFilter { AccurateLogin = request.Login });

            if (user == null)
            {
                return null;
            }

            hashProvider.HashAlgorithmName = user.HashAlgorithm;
            hashProvider.Encoding = Encoding.GetEncoding(user.StringEncoding);

            string hash = hashProvider.GetHash(request.Password);

            if (hash == user.PasswordHash) 
            {
                if (user.IsEmailConfirmed && user.Email != null)
                {
                    await emailSender.SendEmailAsync(new Models.Email { Body = "Someone logged in your acc", Subject = "New login", To = user.Email });
                }

                return mapper.Map<UserDto>(user);
            }

            return null;
        }
    }
}
