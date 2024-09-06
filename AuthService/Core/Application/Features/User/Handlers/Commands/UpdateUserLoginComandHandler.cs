using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Contracts.Infrastructure;
using Application.Contracts.Application;
using Application.Models.Response;

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
            var user = await _userRepository.GetAsync(request.UpdateUserLoginDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            var validator = new UpdateUserLoginDtoValidator();

            var validationResult = await validator.ValidateAsync(request.UpdateUserLoginDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(validationResult.Errors);
            }

            var newLogin = request.UpdateUserLoginDto.NewLogin;

            var foundUser = await _userRepository.GetAsync(new Models.User.UserFilter() { AccurateLogin = newLogin });

            if (foundUser != null)
            { 
                return Response<string>.BadRequestResponse("This login is already taken");
            }

            user.Login = newLogin;

            await _userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Login updated");
        }
    }
}
