﻿using Repository.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    [Table(nameof(Role) + "s")]
    public class Role : IIdObject<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public IEnumerable<User> Users { get; set; } = null!;
    }
}