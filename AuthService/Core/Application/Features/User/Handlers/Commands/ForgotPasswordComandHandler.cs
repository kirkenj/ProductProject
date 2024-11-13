using Application.Contracts.Application;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.User.Requests.Commands;
using Application.Models.User;
using CustomResponse;
using EmailSender.Contracts;
using EmailSender.Models;
using HashProvider.Contracts;
using MediatR;
using Microsoft.Extensions.Options;


namespace Application.Features.User.Handlers.Commands
{
    public class ForgotPasswordComandHandler : IRequestHandler<ForgotPasswordComand, Response<string>>, IPasswordSettingHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly ForgotPasswordSettings _forgotPasswordSettings;

        public ForgotPasswordComandHandler(IUserRepository userRepository, IEmailSender emailSender, IHashProvider hashPrvider, IPasswordGenerator passwordGenerator, IOptions<ForgotPasswordSettings> options)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            HashPrvider = hashPrvider;
            _passwordGenerator = passwordGenerator;
            _forgotPasswordSettings = options.Value;
        }

        public IHashProvider HashPrvider { get; private set; }

        public async Task<Response<string>> Handle(ForgotPasswordComand request, CancellationToken cancellationToken)
        {
            string emailAddress = request.ForgotPasswordDto.Email;

            Domain.Models.User? user = await _userRepository.GetAsync(new UserFilter { AccurateEmail = emailAddress });

            var response = Response<string>.OkResponse("New password was sent on your email", "Success");

            if (user == null)
            {
                return response;
            }

            string newPassword = _passwordGenerator.Generate();

            bool isEmailSent = await _emailSender.SendEmailAsync(new Email
            {
                Body = string.Format(_forgotPasswordSettings.EmailBodyFormat, newPassword),
                Subject = "Password recovery",
                To = user.Email ?? throw new ApplicationException($"User's email is null. User id: '{user.Id}'"),
            });

            if (isEmailSent == false)
            {
                throw new ApplicationException("Couldn't send email");
            }

            (this as IPasswordSettingHandler).SetPassword(newPassword, user);
            await _userRepository.UpdateAsync(user);

            return response;
        }
    }
}
