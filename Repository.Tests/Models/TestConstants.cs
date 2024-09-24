using Cache.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Tests.Models
{
    public static class TestConstants
    {
        public static readonly List<User> DefaultUsers = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Admin",
                Name = "Tom",
                Email = "Tom@gmail.com",
                Address = "Arizona"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "kirk",
                Name = "Kan",
                Email = "Bad@mail.com",
                Address = "Grece"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Samuel1999",
                Name = "Samuel",
                Email = "Samuel1999@bk.ru",
                Address = "Moskow"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Monkey",
                Name = "Alan",
                Email = "Natan@gmail.com",
                Address = "Toronto"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Cat89",
                Name = "Alex",
                Email = "LionTheKing@ya.ru",
                Address = "Minsk"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Boss2012",
                Name = "Rick",
                Email = "Sanchezz@inbox.ru",
                Address = "London"
            }
        };

        public static async Task<TestDbContext> GetDbContextAsync(string? name = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase(name ?? Guid.NewGuid().ToString(), null);
            TestDbContext testDbContext = new(optionsBuilder.Options);

            testDbContext.Users.RemoveRange(testDbContext.Users);

            testDbContext.Users.AddRange(DefaultUsers);

            await testDbContext.SaveChangesAsync();

            testDbContext.SaveChangesFailed += (a, e) => //idk whether it's a good solution
            {
                testDbContext.ChangeTracker.Clear();
            };

            return testDbContext;
        }

        public static CustomCacheOptions CustomCacheOptions => new()
        {
            ConnectionUri = "localhost:3330",
        };

        public static RedisCustomMemoryCacheWithEvents GetEmptyReddis()
        {
            var val = new RedisCustomMemoryCacheWithEvents(CustomCacheOptions);
            val.ClearDb();
            val.DropEvents();
            return val;
        }
    }
}
