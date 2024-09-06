using Repository.Contracts;

namespace Application.DTOs.Role
{
    public class RoleDto : IIdObject<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
