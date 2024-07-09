using Application.DTOs.Role;
using Domain.Common.Interfaces;

namespace Application.DTOs.User
{
    internal class UserListDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string Address { get; set; } = null!;
        public int RoleID { get; set; }
    }
}
