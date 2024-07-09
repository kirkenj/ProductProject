using Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table(nameof(Role) + "s")]
    public class Role : IIdObject<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<User> Users { get; set; } = null!;
    }
}