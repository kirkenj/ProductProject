using Microsoft.Extensions.Logging;
using Moq;
using Repository.Contracts;
using Repository.Models;
using Repository.Tests.Models;

namespace Repository.Tests.GenericFiltrableCachingRepository
{
    public class CachingGetPageContentTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;
        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task Setup()
        {
            var redis = TestConstants.GetEmptyReddis();

            redis.ClearDb();

            _customMemoryCache = redis;

            _testDbContext = await TestConstants.GetDbContextAsync("TestDb");

            var MockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _repository = new GenericCachingRepository<User, Guid>(_testDbContext, _customMemoryCache, MockLogger);
        }

        [SetUp]
        public void SetUp()
        {
            _customMemoryCache.ClearDb();
            _customMemoryCache.DropEvents();
        }

        [Test]
        public async Task GetPageContentTests_NullPageNullSize_ReturnsAllValuesCachesAllValues()
        {
            List<User> cachedUsers = new List<User>();

            ICollection<User>? cachedUsersAsRange = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal)
                {
                    cachedUsers.Add(uVal);
                }
                else if (value is ICollection<User> uCollection)
                {
                    cachedUsersAsRange = uCollection;
                }
            };

            var users = await _repository.GetPageContent(null, null);

            Assert.That(users, Is.EquivalentTo(Users));
            Assert.That(users, Is.EquivalentTo(cachedUsersAsRange));
            Assert.That(users, Is.EquivalentTo(cachedUsers));
        }

        [Test]
        public async Task GetPageContentTests_NullPageNotNullSize_ReturnsAllValues()
        {
            List<User> cachedUsers = new List<User>();

            ICollection<User>? cachedUsersAsRange = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal)
                {
                    cachedUsers.Add(uVal);
                }
                else if (value is ICollection<User> uCollection)
                {
                    cachedUsersAsRange = uCollection;
                }
            };

            var users = await _repository.GetPageContent(null, 1);

            Assert.That(users, Is.EquivalentTo(Users));
            Assert.That(users, Is.EquivalentTo(cachedUsersAsRange));
            Assert.That(users, Is.EquivalentTo(cachedUsers));
        }

        [Test]
        public async Task GetPageContentTests_NotNullPageNullSize_ReturnsAllValues()
        {
            List<User> cachedUsers = new List<User>();

            ICollection<User>? cachedUsersAsRange = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal)
                {
                    cachedUsers.Add(uVal);
                }
                else if (value is ICollection<User> uCollection)
                {
                    cachedUsersAsRange = uCollection;
                }
            };

            var users = await _repository.GetPageContent(1, null);

            Assert.That(users, Is.EquivalentTo(Users));
            Assert.That(users, Is.EquivalentTo(cachedUsersAsRange));
            Assert.That(users, Is.EquivalentTo(cachedUsers));
        }

        [TestCase(1, 1, 0, 1)]
        [TestCase(2, 1, 1, 1)]
        [TestCase(2, 2, 2, 2)]
        [TestCase(1, 3, 0, 3)]
        [TestCase(2, 3, 3, 3)]
        [TestCase(3, 2, 4, 2)]
        public async Task GetPageContentTests_NotNullPageNullSize_ReturnsAllValues(int page, int pageSize, int expectedResultIndex, int expectedResultCount)
        {
            List<User> cachedUsers = new List<User>();

            ICollection<User>? cachedUsersAsRange = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal)
                {
                    cachedUsers.Add(uVal);
                }
                else if (value is ICollection<User> uCollection)
                {
                    cachedUsersAsRange = uCollection;
                }
            };

            var users = await _repository.GetPageContent(page, pageSize);

            var expectedResult = Users.GetRange(expectedResultIndex, expectedResultCount);

            Assert.That(users, Is.EquivalentTo(expectedResult));
            Assert.That(users, Is.EquivalentTo(cachedUsersAsRange));
            Assert.That(users, Is.EquivalentTo(cachedUsers));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}