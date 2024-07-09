using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class UpdateUserRoleDTO : IRoleEditIingDto
    {
        public Guid UserId { get; set; }
        public int RoleID { get; set; }
    }
}