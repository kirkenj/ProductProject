using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Role adminRole = new() { Id = 1, Name = "Admin" };
            Role regularRole = new() { Id = 2, Name = "Regular" };
            modelBuilder.Entity<Role>().HasData(adminRole);
            modelBuilder.Entity<Role>().HasData(regularRole);

            string hashAlgorithmName = "MD5";
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashAlgorithmName) ?? throw new Exception($"Hash algorithm not found {hashAlgorithmName}");
            Encoding encoding = Encoding.UTF8;
            
            ((string login, string password) userData, int roleID)[] startUsersArray = new [] 
            {
                (("admin", "admin"), adminRole.Id), 
                (("user", "user"), regularRole.Id)
            };

            foreach (var item in startUsersArray)
            {
                var adminPwdBytes = encoding.GetBytes(item.userData.password);
                var adminPwdHash = hashAlgorithm.ComputeHash(adminPwdBytes);
                var adminPwdHashString = encoding.GetString(adminPwdHash);
                modelBuilder.Entity<User>().HasData(new User { Address = "Confidential", Email = null, Login = item.userData.login, PasswordHash = adminPwdHashString, StringEncoding = encoding.EncodingName, Id = Guid.NewGuid(), HashAlgorithm = hashAlgorithmName, RoleID = item.roleID });
            }


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }
    }
}
