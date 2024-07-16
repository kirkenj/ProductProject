using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Application.Models;

namespace Application.Features.User.Handlers.Commands
{
    public class ConfirmEmailComandHandler : IRequestHandler<ConfirmEmailComand, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;

        public ConfirmEmailComandHandler(IUserRepository userRepository, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        public async Task<Response> Handle(ConfirmEmailComand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.ConfirmEmailDto.UserId);

            var response = new Response();

            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            if (user.IsEmailConfirmed)
            {

                throw new ApplicationException("Email is already confirmed");
            }

            if (user.Email == null)
            {
                throw new ApplicationException("Coupled email to this user is null");
            }

            var storedToken = _memoryCache.Get(user.Email);

            if (storedToken == null)
            {
                response.Success = false;
                response.Message = "Token expired";
                response.Status = 400;
                return response;
            }

            if (storedToken.ToString() == request.ConfirmEmailDto.Key) 
            { 
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(user);
                _memoryCache.Remove(user.Email);


                response.Success = true;
                response.Message = "Email confirmed";
                response.Status = 200;
                return response;
            }



            response.Success = true;
            response.Message = "Wrong token";
            response.Status = 400;
            return response;
        }
    }
}
