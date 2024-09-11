using Application.DTOs.User.Interfaces;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UpdateUserRoleDTO : IRoleDto, IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public int RoleID { get; set; }
    }
}