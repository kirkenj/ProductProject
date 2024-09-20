using Microsoft.EntityFrameworkCore;
using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class AddAsyncTests
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
        public void AddAsync_UserIsNull_ThrowsArgumentNullException()
        {
            var func = async () => await _repository.AddAsync(null);

            Assert.That(func, Throws.ArgumentNullException);
        }

        [Test]
        public void AddAsyncTests_UserAlreadyInDbSet_ThrowsArgumentException()
        {
            var func = async () => await _repository.AddAsync(_users.First());

            Assert.That(func, Throws.ArgumentException);
        }

        [Test]
        public void AddAsyncTests_IDIsTaken_ThrowsInvalidOperationException()
        {
            var user = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.ru",
                Login = "BBCReporter",
                Id = _users.First().Id
            };

            var func = async () => await _repository.AddAsync(user);

            Assert.That(func, Throws.InvalidOperationException);
        }        
    }
}