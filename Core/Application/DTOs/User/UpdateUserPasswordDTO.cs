using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class UpdateUserPasswordDTO : IUpdateUserPasswordDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; } = null!;
    }
}