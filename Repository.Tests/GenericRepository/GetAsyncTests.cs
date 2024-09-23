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
        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task Setup()
        {
            _testDbContext = await TestConstants.GetDbContextAsync();
            _repository = new GenericRepository<User, Guid>(_testDbContext);
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
            var user = Users.First();

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