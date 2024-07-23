using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Application.Contracts.Infrastructure;
using System.Text;
using Application.Models.User;
using Application.Models.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.User.Handlers.Queries
{
    public class LoginHandler : IRequestHandler<LoginRequest, Response<string?>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IJwtService _jwtService;
        private readonly IMemoryCache _memoryCache;
        private readonly IRoleRepository _roleRepository;

        public LoginHandler(IUserRepository userRepository, IRoleRepository roleRepository, IHashProvider hashProvider, IMapper mapper, IMemoryCache memoryCache, IEmailSender emailSender, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _jwtService = jwtService;
            _memoryCache = memoryCache;
        }

        public async Task<Response<string?>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var loginEmail = request.LoginDto.Email;
            var cacheAccessKey = CacheKeyGenerator.CacheKeyGenerator.KeyForRegistrationCaching(loginEmail);
            var cachedUserValue = _memoryCache.Get(cacheAccessKey);

            bool isRegistration = cachedUserValue != null && cachedUserValue is Domain.Models.User;

            Domain.Models.User? userToHandle = isRegistration ? 
                (Domain.Models.User?)cachedUserValue 
                : await _userRepository.GetAsync(new UserFilter { Email = loginEmail }, true);

            if (userToHandle == null)
            {
                return Response<string?>.NotFoundResponse(nameof(loginEmail), true);
            }

            _hashProvider.HashAlgorithmName = userToHandle.HashAlgorithm;
            _hashProvider.Encoding = Encoding.GetEncoding(userToHandle.StringEncoding);

            string loginPasswordHash = _hashProvider.GetHash(request.LoginDto.Password);

            if (loginPasswordHash != userToHandle.PasswordHash)
            { 
                return Response<string?>.BadRequestResponse("Wrong password");
            }

            if (isRegistration == true)
            {
                await _userRepository.AddAsync(userToHandle);
                _memoryCache.Remove(cacheAccessKey);
                userToHandle.Role = await _roleRepository.GetAsync(userToHandle.RoleID) ?? throw new ApplicationException();

                await _emailSender.SendEmailAsync(new()
                {
                    To = userToHandle.Email ?? throw new ApplicationException($"User email is null, Id = {userToHandle.Id}, userFromCache = {isRegistration}"),
                    Subject = "First login",
                    Body = "Account confirmed"
                });
            }

            var mapped = _mapper.Map<UserDto>(userToHandle);

            return Response<string?>.OkResponse(_jwtService.GetToken(mapped), "Success");
        }
    }
}
