using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Email;
using Application.Contracts.Infrastructure;
using Application.Models.Response;
using Application.Models.User;
using Application.Contracts.Application;
using EmailSender.Contracts;
using EmailSender.Models;


namespace Application.Features.User.Handlers.Commands
{
    public class ForgotPasswordComandHandler : IRequestHandler<ForgotPasswordComand, Response<string>>, IPasswordSettingHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordGenerator _passwordGenerator;

        public ForgotPasswordComandHandler(IUserRepository userRepository, IEmailSender emailSender, IHashProvider hashPrvider, IPasswordGenerator passwordGenerator)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            HashPrvider = hashPrvider;
            _passwordGenerator = passwordGenerator;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<string>> Handle(ForgotPasswordComand request, CancellationToken cancellationToken)
        {
            var emailAddress = request.ForgotPasswordDto.Email;

            var user = await _userRepository.GetAsync(new UserFilter { Email = emailAddress});

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            var newPassword = _passwordGenerator.Generate();

            if (user.Email == null)
            { 
                return Response<string>.BadRequestResponse("Your email is null. Contact administration");
            }

            var emailResult = await _emailSender.SendEmailAsync( new Email
                    {
                        Body = $"Dear {user.Name}. Your new password is: {newPassword}",
                        Subject = "Password recovery",
                        To = user.Email
                    });

            if (emailResult == true)
            {
                try
                {
                    (this as IPasswordSettingHandler).SetPassword(newPassword, user);
                    await _userRepository.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"EMAIL WAS SENT, BUT PASSWORD WAS NOT UPDATED. USERID: {user.Id}", ex);
                }

                return Response<string>.OkResponse("Success", "New password was sent on your emial");
            }

            return Response<string>.ServerErrorResponse("Server side error");
        }
    }
}
