using Application.Features.User.Requests.Commands;
using Application.Contracts.Persistence;
using Application.Exceptions;
using MediatR;
using Application.Models.Email;
using Application.Models.User;
using Application.Contracts.Infrastructure;

namespace Application.Features.User.Handlers.Commands
{
    public class UpdateUserEmailConfirmStatusComandHandler : IRequestHandler<UpdateUserEmailConfirmStatusComand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;

        public UpdateUserEmailConfirmStatusComandHandler(IUserRepository userRepository, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.emailSender = emailSender;
        }

        public async Task<Unit> Handle(UpdateUserEmailConfirmStatusComand request, CancellationToken cancellationToken)
        {
            var emailAddress = request.UpdateUserEmailConfirmStatusDto.Email;

            var user = await userRepository.GetAsync(new UserFilter { Email = emailAddress });
            
            if (user == null)
            {
                throw new NotFoundException(nameof(user), $"{nameof(emailAddress)} = {emailAddress}");
            }

            user.IsEmailConfirmed = request.UpdateUserEmailConfirmStatusDto.Status;
            
            await userRepository.UpdateAsync(user);

            var email = new Email{ Body = "Email confirmed", Subject = "Email confirmation", To = emailAddress };

            try
            {
                await emailSender.SendEmailAsync(email);
            }
            catch //(Exception ex) 
            { 
            }


            return Unit.Value;
        }
    }
}
