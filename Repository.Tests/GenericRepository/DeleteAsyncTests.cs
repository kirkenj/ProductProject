using Microsoft.EntityFrameworkCore;
using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class DeleteAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;

        private readonly List<User> _users = new()
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
                Login = "User",
                Name = "Nick",
                Email = "Crazy@hotmail.com",
                Address = "Grece"
            }
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestDb", null).EnableServiceProviderCaching();
            _testDbContext = new(optionsBuilder.Options);
            _repository = new GenericRepository<User, Guid>(_testDbContext);

            _testDbContext.Users.AddRange(_users);
            _testDbContext.SaveChanges();
        }

        [Test]
        public void DeleteAsync_UserIsNull_ThrowsArgumentNullException()
        {
            var func = async () => await _repository.AddAsync(null);

            Assert.That(func, Throws.ArgumentNullException);
        }

        [Test]
        public async Task DeleteAsync_UserInDbSet_ThrowsArgumentException()
        {
            var user = _users.First();

            await _repository.DeleteAsync(user);
            
            var repUsers = await _repository.GetAllAsync();
            
            Assert.That(repUsers, Does.Not.Contain(user));
        }

        [Test]
        public async Task DeleteAsync_UserNotInDbSet_ThrowsArgumentException()
        {
            var user = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.ru",
                Login = "BBCReporter",
                Id = Guid.NewGuid()
            };

            var func = async () => await _repository.DeleteAsync(user);
            
            
            Assert.That(func, Throws.Exception);
        }
    }
}