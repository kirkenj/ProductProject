using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class UpdateUserPasswordDto : IUpdateUserPasswordDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}