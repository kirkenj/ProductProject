using Microsoft.EntityFrameworkCore;
using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class GetAsyncTests
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
        public async Task GetAsync_IDdefault_ReturnsNull()
        {
            var users = await _repository.GetAsync(default);

            Assert.That(users, Is.Null);
        }

        [Test]
        public async Task GetAsync_IDNotContained_ReturnsNull()
        {
            var user = await _repository.GetAsync(Guid.NewGuid());

            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task GetAsync_IDContained_ReturnsTheUser()
        {
            var user = _users.First();

            var users = await _repository.GetAsync(user.Id);

            Assert.That(users, Is.EqualTo(user));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}