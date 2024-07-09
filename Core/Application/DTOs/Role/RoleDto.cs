using Application.DTOs.User;
using Domain.Common.Interfaces;

namespace Application.DTOs.Role
{
    public class RoleDto : IIdObject<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<UserDto>? Users { get; set; }
    }
}
