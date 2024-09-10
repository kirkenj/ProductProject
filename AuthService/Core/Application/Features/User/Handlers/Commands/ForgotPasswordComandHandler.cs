using Application.Contracts.Application;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using Application.Models.User;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using MediatR;


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
            string emailAddress = request.ForgotPasswordDto.Email;

            Domain.Models.User? user = await _userRepository.GetAsync(new UserFilter { Email = emailAddress });

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(request.ForgotPasswordDto.Email), true);
            }

            string newPassword = _passwordGenerator.Generate();

            bool isEmailSent = await _emailSender.SendEmailAsync(new Email
            {
                Body = $"Dear {user.Name}. Your new password is: {newPassword}",
                Subject = "Password recovery",
                To = user.Email ?? throw new ApplicationException($"User's email is null. User id: '{user.Id}'"),
            });

            if (isEmailSent == false)
            {
                throw new ApplicationException("Couldn't send email");
            }

            try
            {
                (this as IPasswordSettingHandler).SetPassword(newPassword, user);
                await _userRepository.UpdateAsync(user);
                return Response<string>.OkResponse("New password was sent on your emial", "Success");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"EMAIL WAS SENT, BUT PASSWORD WAS NOT UPDATED. USERID: {user.Id}", ex);
            }
        }
    }
}
