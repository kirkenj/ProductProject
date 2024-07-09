using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class CreateUserDto : IRoleEditIingDto, IUpdateUserPasswordDto
    {
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string Address { get; set; } = null!;
        public int RoleID { get; set; }
        public string Password { get; set; } = null!;
    }
}
