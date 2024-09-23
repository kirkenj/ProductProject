using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class GetAllAsyncTests
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
        public async Task GetAllAsync_ReturnsValues()
        {
            var users = await _repository.GetAllAsync();

            Assert.That(users, Is.EquivalentTo(Users));
        }

        [TearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}