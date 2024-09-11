using Application.DTOs.Role;
using Application.DTOs.User.Interfaces;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UserDto : IIdObject<Guid>, IUserInfoDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public RoleDto Role { get; set; } = null!;
    }
}
