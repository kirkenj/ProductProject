using Application.Contracts.Application;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using Application.Features.User.Requests.Commands;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserPasswordComandHandler : IRequestHandler<UpdateUserPasswordComand, Response<string>>, IPasswordSettingHandler
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserPasswordComandHandler(IUserRepository userRepository, IHashProvider hashProvider)
        {
            _userRepository = userRepository;
            HashPrvider = hashProvider;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<string>> Handle(UpdateUserPasswordComand request, CancellationToken cancellationToken)
        {
            Domain.Models.User? user = await _userRepository.GetAsync(request.UpdateUserPasswordDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            var validator = new UpdateUserPasswordDTOValidator();

            var validationResult = await validator.ValidateAsync(request.UpdateUserPasswordDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return Response<string>.BadRequestResponse(string.Join("; ", validationResult.Errors.Select(e =>e.ErrorMessage)));
            }

            (this as IPasswordSettingHandler).SetPassword(request.UpdateUserPasswordDto.NewPassword, user);

            await _userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Password updated");
        }
    }
}
