using Application.DTOs.Role;
using Domain.Common.Interfaces;

namespace Application.DTOs.User
{
    public class UserDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int RoleID { get; set; }
        public RoleDto Role { get; set; } = null!;
    }
}
