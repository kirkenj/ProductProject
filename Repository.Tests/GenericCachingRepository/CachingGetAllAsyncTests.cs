using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Repository.Tests.GenericCachingRepository
{
    public class CachingGetAllAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
             _customMemoryCache = TestConstants.GetReddis();

            _testDbContext = await TestConstants.GetDbContextAsync();

            var MockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _repository = new GenericCachingRepository<User, Guid>(_testDbContext, _customMemoryCache, MockLogger);
        }

        [SetUp]
        public void Setup()
        {
            _customMemoryCache.ClearDb();
            _customMemoryCache.DropEvents();
        }

        [Test]
        public async Task GetAllAsync_ReturnsValuesAddsValueToCache()
        {
            //arrange
            var cachedUsers = new List<User>();

            ICollection<User>? cachedRange = null;

            _customMemoryCache.OnSet += (key, value, span) =>
            {
                if (value is User uVal)
                {
                    cachedUsers.Add(uVal);
                }
                else if (value is ICollection<User> idVal)
                {
                    cachedRange = idVal;
                }
            };

            //act
            var users = await _repository.GetAllAsync();

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(users, Is.EquivalentTo(_testDbContext.Users.ToArray()));

                Assert.That(users, Is.EquivalentTo(cachedRange));

                Assert.That(users, Is.EquivalentTo(cachedUsers));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}