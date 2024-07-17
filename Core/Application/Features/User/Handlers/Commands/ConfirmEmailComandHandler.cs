using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Commands
{
    public class ConfirmEmailComandHandler : IRequestHandler<ConfirmEmailComand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public ConfirmEmailComandHandler(IUserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Response<string>> Handle(ConfirmEmailComand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.ConfirmEmailDto.UserId);

            var response = new Response<string>();

            if (user == null)
            {
                //throw new ApplicationException("User not found");
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            if (user.IsEmailConfirmed)
            {
                return Response<string>.BadRequestResponse("This email is already confirmed");
            }

            if (user.Email == null)
            {
                return Response<string>.BadRequestResponse("Coupled email to this user is not set");
            }

            var storedToken = _memoryCache.Get(user.Email);

            if (storedToken == null)
            {
                return Response<string>.BadRequestResponse("Token expired");
            }

            if (storedToken.ToString() == request.ConfirmEmailDto.Key) 
            { 
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(user);
                _memoryCache.Remove(user.Email);

                return Response<string>.OkResponse("Ok", "Email confirmed");
            }

            return Response<string>.BadRequestResponse("You sent wrong token");
        }
    }
}
