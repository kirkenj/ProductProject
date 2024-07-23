using Application.DTOs.Role;
using Domain.Common.Interfaces;

namespace Application.DTOs.User
{
    public class UserListDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public RoleDto Role { get; set; } = null!;
    }
}
