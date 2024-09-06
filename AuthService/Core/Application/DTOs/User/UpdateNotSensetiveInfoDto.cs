using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UpdateNotSensetiveInfoDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
