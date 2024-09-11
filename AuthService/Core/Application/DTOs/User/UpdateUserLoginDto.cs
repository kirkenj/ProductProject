using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UpdateUserLoginDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string NewLogin { get; set; } = null!;
    }
}