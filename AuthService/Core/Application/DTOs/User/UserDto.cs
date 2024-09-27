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


        public override bool Equals(object? obj)
        {
            if (obj is UserDto dto)
                return this.Id == dto.Id &&
                    this.Login == dto.Login &&
                    this.Email == dto.Email &&
                    this.Name == dto.Name &&
                    this.Address == dto.Address &&
                    this.Role.Equals(dto.Role);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Login, Email, Name, Address, Role);
        }
    }
}
