using Application.DTOs.User.Interfaces;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class SendTokenToUpdateUserEmailDto : IEmailUpdateDto, IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
    }
}
