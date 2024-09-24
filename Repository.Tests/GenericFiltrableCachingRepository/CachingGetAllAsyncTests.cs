using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Repository.Tests.GenericFiltrableCachingRepository
{
    public class CachingGetAllAsyncTests
    {
        private ICollection<User> Users => _testDbContext.Users.ToArray();
        private GenericFiltrableCachingRepository<User, Guid, UserFilter> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
             _customMemoryCache = TestConstants.GetReddis();

            _testDbContext = await TestConstants.GetDbContextAsync();

            var MockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _repository = new GenericFiltrableCachingRepository<User, Guid, UserFilter>(_testDbContext, _customMemoryCache, MockLogger, UserFilter.GetFilteredSet);
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
                Assert.That(users, Is.EquivalentTo(Users));

                Assert.That(users, Is.EquivalentTo(cachedRange));

                Assert.That(users, Is.EquivalentTo(cachedUsers));
            });
        }

        [Test]
        public async Task GetAllAsync_MultipleGetFirstTimeowt_SecondGetFromRep()
        {
            //arrange
            ICollection<User>? firstPreCachedRange = null;
            ICollection<User>? secondPreCachedRange = null;

            int cacheGetCounter = 0;

            _customMemoryCache.OnGet += (key, value) =>
            {
                cacheGetCounter++;

                if (value is not ICollection<User> uColl)
                {
                    return;
                }

                if (cacheGetCounter == 1)
                {
                    firstPreCachedRange = uColl;
                }

                if (cacheGetCounter == 2)
                {
                    secondPreCachedRange = uColl;
                }
            };

            //act
            var usersFirst = await _repository.GetAllAsync();

            await Task.Delay(_repository.ÑacheTimeoutMiliseconds);

            var usersSecond = await _repository.GetAllAsync();

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(cacheGetCounter, Is.EqualTo(2));

                Assert.That(usersFirst, Is.EquivalentTo(Users));

                Assert.That(usersFirst, Is.EquivalentTo(usersSecond));

                Assert.That(usersFirst, Is.EquivalentTo(_testDbContext.Users));

                Assert.That(firstPreCachedRange, Is.Null);

                Assert.That(secondPreCachedRange, Is.Null);
            });
        }

        [Test]
        public async Task GetAllAsync_MultipleGet_SecondGetFromCache()
        {
            //arrange

            ICollection<User>? firstPreCachedRange = null;
            ICollection<User>? secondPreCachedRange = null;

            int cacheCounter = 0;

            _customMemoryCache.OnGet += (key, value) =>
            {
                cacheCounter++;

                if (value is not ICollection<User> uColl)
                {
                    return;
                }

                if (cacheCounter == 1)
                {
                    firstPreCachedRange = uColl;
                }

                if (cacheCounter == 2)
                {
                    secondPreCachedRange = uColl;
                }
            };

            //act
            var usersFirst = await _repository.GetAllAsync();

            var usersSecond = await _repository.GetAllAsync();

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(cacheCounter, Is.EqualTo(2));

                Assert.That(usersFirst, Is.EquivalentTo(Users));

                Assert.That(usersFirst, Is.EquivalentTo(usersSecond));

                Assert.That(firstPreCachedRange, Is.Null);

                Assert.That(secondPreCachedRange, Is.EquivalentTo(usersFirst));

                Assert.That(secondPreCachedRange, Is.EquivalentTo(usersSecond));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}