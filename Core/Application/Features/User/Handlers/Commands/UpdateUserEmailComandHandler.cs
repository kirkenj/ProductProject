using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using MediatR;
using Application.Models.Email;
using Application.Contracts.Infrastructure;
using Application.Models.Response;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserEmailComandHandler : IRequestHandler<UpdateUserEmailComand, Response<string>>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;

        public UpdateUserEmailComandHandler(IUserRepository userRepository, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.emailSender = emailSender;
        }

        public async Task<Response<string>> Handle(UpdateUserEmailComand request, CancellationToken cancellationToken)
        {
            var emailAddress = request.UpdateUserEmailDto.Email;

            var user = await userRepository.GetAsync(request.UpdateUserEmailDto.Id);

            if (user == null)
            {
                return Response<string>.NotFoundResponse(nameof(user.Id), true);
            }

            if (user.IsEmailConfirmed && user.Email != null)
            {
                await emailSender.SendEmailAsync(
                    new Email
                    {
                        Body = $"Dear {user.Login}. Your email is being updated. Your email confirmation status dropped.",
                        Subject = "Email updated",
                        To = user.Email
                    }
                    );
            }

            user.Email = emailAddress;
            user.IsEmailConfirmed = false;
            await userRepository.UpdateAsync(user);

            return Response<string>.OkResponse("Ok", "Email changed. Confirmation status dropped");
        }
    }
}
