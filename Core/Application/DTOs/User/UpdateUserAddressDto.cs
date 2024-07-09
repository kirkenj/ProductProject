using Domain.Common.Interfaces;

namespace Application.DTOs.User
{
    public class UpdateUserAddressDto : IIdObject<Guid>
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = null!;
    }
}
