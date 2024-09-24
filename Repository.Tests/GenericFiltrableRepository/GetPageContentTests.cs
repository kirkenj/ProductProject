using Repository.Contracts;
using Repository.Models;
using Repository.Tests.Models;

namespace Repository.Tests.GenericFiltrableRepository
{
    public class GetPageContentTests
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
        public async Task GetPageContentTests_NullPageNullSize_ReturnsAllValues()
        {
            var users = await _repository.GetPageContent(null, null);

            Assert.That(users, Is.EqualTo(_testDbContext.Users.ToArray()));
        }

        [Test]
        public async Task GetPageContentTests_NullPageNotNullSize_ReturnsAllValues()
        {
            var users = await _repository.GetPageContent(null, 1);

            Assert.That(users, Is.EqualTo(_testDbContext.Users.ToArray()));
        }

        [Test]
        public async Task GetPageContentTests_NotNullPageNullSize_ReturnsAllValues()
        {
            var users = await _repository.GetPageContent(1, null);

            Assert.That(users, Is.EqualTo(_testDbContext.Users.ToArray()));
        }

        [TestCase(1, 1, 0, 1)]
        [TestCase(2, 1, 1, 1)]
        [TestCase(2, 2, 2, 2)]
        [TestCase(1, 3, 0, 3)]
        [TestCase(2, 3, 3, 3)]
        [TestCase(3, 2, 4, 2)]
        public async Task GetPageContentTests_NotNullPageNullSize_ReturnsAllValues(int page, int pageSize, int expectedResultIndex, int expectedResultCount)
        {
            var users = await _repository.GetPageContent(page, pageSize);

            var expectedResult = Users.GetRange(expectedResultIndex, expectedResultCount);

            Assert.That(users, Is.EqualTo(expectedResult));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}