using Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table(nameof(User) + "s")]
    public class User : IIdObject<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string HashAlgorithm { get; set; } = null!;
        public string StringEncoding { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RoleID { get; set; }
        [ForeignKey(nameof(RoleID))]
        public Role Role { get; set; } = null!;
    }
}
