using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class CreateUserDto : IUpdateUserPasswordDto
    {
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string Address { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
