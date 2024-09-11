using Application.Contracts.Application;
using HashProvider.Contracts;
using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserLoginComandHandler : IRequestHandler<UpdateUserLoginComand, Response<string>>, IPasswordSettingHandler
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserLoginComandHandler(IUserRepository userRepository, IHashProvider hashProvider)
        {
            _userRepository = userRepository;
            HashPrvider = hashProvider;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<string>> Handle(UpdateUserLoginComand request, CancellationToken cancellationToken)
        {
            var newLogin = request.UpdateUserLoginDto.NewLogin;

            Domain.Models.User? userWithNewlogin = await _userRepository.GetAsync(new Models.User.UserFilter() { AccurateLogin = newLogin });

            if (userWithNewlogin != null)
            {
                return Response<string>.BadRequestResponse("This login is already taken");
            }

            Domain.Models.User? userToUpdate = await _userRepository.GetAsync(request.UpdateUserLoginDto.Id);

            if (userToUpdate == null)
            {
                return Response<string>.NotFoundResponse(nameof(userToUpdate.Id), true);
            }

            userToUpdate.Login = newLogin;

            await _userRepository.UpdateAsync(userToUpdate);

            return Response<string>.OkResponse("Ok", "Login updated");
        }
    }
}
