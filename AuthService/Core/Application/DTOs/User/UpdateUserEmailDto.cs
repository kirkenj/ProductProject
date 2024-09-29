using Application.DTOs.User.Interfaces;
using Repository.Contracts;

namespace Application.DTOs.User
{
    public class UpdateUserEmailDto : IEmailUpdateDto, IIdObject<Guid>
    {
        /// <summary>
        /// User's Id where email will be updated
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// New email
        /// </summary>
        public string Email { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj is UpdateUserEmailDto dto)
            {
                return dto.Email == Email && dto.Id.Equals(Id);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Email);
        }
    }
}
