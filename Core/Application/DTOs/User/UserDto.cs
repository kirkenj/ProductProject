using Domain.Common.Interfaces;

namespace Application.DTOs.User
{
    public class UserDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string Address { get; set; } = null!;
        public int RoleID { get; set; }
        public string HasgAlgorithm { get; set; } = null!;
        public string StringEncoding { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
