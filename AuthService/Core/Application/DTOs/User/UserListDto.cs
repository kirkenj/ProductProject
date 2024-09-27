using Application.DTOs.Role;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UserListDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleDto Role { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj is UserListDto dto)
            {
                var cmpResult = this.Id == dto.Id &&
                    this.Login == dto.Login &&
                    this.Email == dto.Email &&
                    this.Role.Equals(dto.Role);
                return cmpResult;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Login, Email, Role);
        }
    }
}
