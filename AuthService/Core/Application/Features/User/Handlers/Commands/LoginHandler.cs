using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Models.CacheKeyGenerator;
using Application.Models.User;
using AutoMapper;
using Cache.Contracts;
using CustomResponse;
using EmailSender.Contracts;
using HashProvider.Contracts;
using MediatR;
using System.Text;

namespace Application.Features.User.Handlers.Commands
{
    public class LoginHandler : IRequestHandler<LoginRequest, Response<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ICustomMemoryCache _memoryCache;
        private readonly IRoleRepository _roleRepository;

        public LoginHandler(IUserRepository userRepository, IRoleRepository roleRepository, IHashProvider hashProvider, IMapper mapper, ICustomMemoryCache memoryCache, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        public async Task<Response<UserDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            string loginEmail = request.LoginDto.Email;
            string cacheKey = CacheKeyGenerator.KeyForRegistrationCaching(loginEmail);
            Domain.Models.User? cachedUserValue = await _memoryCache.GetAsync<Domain.Models.User>(cacheKey);

            bool isRegistration = cachedUserValue != null;

            var userToHandle = isRegistration ?
                cachedUserValue
                : await _userRepository.GetAsync(new UserFilter { Email = loginEmail });

            if (userToHandle == null)
            {
                return Response<UserDto>.NotFoundResponse(nameof(loginEmail), true);
            }

            _hashProvider.HashAlgorithmName = userToHandle.HashAlgorithm;
            _hashProvider.Encoding = Encoding.GetEncoding(userToHandle.StringEncoding);

            string loginPasswordHash = _hashProvider.GetHash(request.LoginDto.Password);

            if (loginPasswordHash != userToHandle.PasswordHash)
            {
                return Response<UserDto>.BadRequestResponse("Wrong password");
            }

            if (isRegistration == true)
            {
                await RegisterUser(userToHandle);
                await _memoryCache.RemoveAsync(cacheKey);
            }

            UserDto userDto = _mapper.Map<UserDto>(userToHandle);

            return Response<UserDto>.OkResponse(userDto, "Success");
        }

        private async Task RegisterUser(Domain.Models.User userToHandle)
        {
            await _userRepository.AddAsync(userToHandle);

            userToHandle.Role = await _roleRepository.GetAsync(userToHandle.RoleID) ?? throw new ApplicationException();

            var isEmailSent = await _emailSender.SendEmailAsync(new()
            {
                To = userToHandle.Email ?? throw new ApplicationException($"User email is null, Id = {userToHandle.Id}, userFromCache = {true}"),
                Subject = "First login",
                Body = "Account confirmed"
            });

            if (isEmailSent == false)
            {
                throw new ApplicationException("Couldn't send email");
            }
        }
    }
}
