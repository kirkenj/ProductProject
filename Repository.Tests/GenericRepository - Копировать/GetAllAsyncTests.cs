using Microsoft.EntityFrameworkCore;
using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class GetAllAsyncTests
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
        public async Task GetAllAsync_ReturnsValues()
        {
            var users = await _repository.GetAllAsync();

            Assert.That(users, Is.EqualTo(_testDbContext.Users.ToArray()));
        }

        [TearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}