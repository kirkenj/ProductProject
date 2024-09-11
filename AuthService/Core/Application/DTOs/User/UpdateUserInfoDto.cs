using Application.DTOs.User.Interfaces;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UpdateUserInfoDto : IIdObject<Guid>, IUserInfoDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
