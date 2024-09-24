using Cache.Contracts;
using Cache.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Contracts;
using Repository.Models;
using Repository.Tests.Models;
using static Repository.Tests.Models.RedisCustomMemoryCacheWithEvents;

namespace Repository.Tests.GenericCachingRepository
{
    public class CachingGetAsyncTests
    {
        private IGenericRepository<User, Guid> Repository => _genericCachingRepository;

        private GenericCachingRepository<User, Guid> _genericCachingRepository = null!;

        private TestDbContext _testDbContext = null!;

        private RedisCustomMemoryCacheWithEvents _redis;
        private ICustomMemoryCache CustomMemoryCache => _redis;

        private ILogger<GenericCachingRepository<User, Guid>> _logger = null!;

        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task Setup()
        {
            _redis = new RedisCustomMemoryCacheWithEvents(new CustomCacheOptions { ConnectionUri = "localhost:3330" });

            _testDbContext = await TestConstants.GetDbContextAsync();

            var MockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _genericCachingRepository = new GenericCachingRepository<User, Guid>(_testDbContext, CustomMemoryCache, MockLogger);
        }


        [SetUp]
        public void SetUp()
        {
            _redis.DropEvents();
            _redis.ClearDb();
        }

        [Test]
        public async Task GetAsync_IDdefault_ReturnsNull()
        {
            var users = await Repository.GetAsync(default);

            Assert.That(users, Is.Null);
        }

        [Test]
        public async Task GetAsync_IDNotContained_ReturnsNull()
        {
            var user = await Repository.GetAsync(Guid.NewGuid());

            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task GetAsync_IDContained_ReturnsTheUser()
        {
            var user = Users[Random.Shared.Next(0, Users.Count)];

            var users = await Repository.GetAsync(user.Id);

            Assert.That(users, Is.EqualTo(user));
        }


        [Test]
        public async Task GetAsync_ValueExcists_NotFoundInCacheReturnsValueAddsValueToCache()
        {
            //arrange

            Guid userId = Users[Random.Shared.Next(0, Users.Count)].Id;

            User? preCachedResult = null;

            User? valueAddedToCache = null;

            OnGetHandler onGetHandler = (key, result) =>
            {
                if (result is User uResult)
                {
                    preCachedResult = uResult;
                }
            };


            OnSetHandler onSetHandler = (key, value, span) =>
            {
                if (value is User uResult)
                {
                    valueAddedToCache = uResult;
                }
            };

            _redis.OnSet += onSetHandler;
            _redis.OnGet += onGetHandler;

            //act

            var user = await Repository.GetAsync(userId);

            //assert

            Assert.Multiple(() =>
            {
                Assert.That(preCachedResult, Is.Null, "Precached Result Check");
                Assert.That(valueAddedToCache, Is.EqualTo(user), "After act cached result");
            });
        }


        [Test]
        public async Task GetAsync_GetTwiseInARowWithDelay_SecondValueGotFromContext()
        {
            //arrange

            Guid userId = Users[Random.Shared.Next(0, Users.Count)].Id;

            User? firstPreCachedResult = null;

            User? secondPreCachedResult = null;

            int cacheGetCounter = 0;

            _redis.OnGet += (key, result) =>
            {
                cacheGetCounter++;

                if (result is not User uResult)
                {
                    return;
                }

                if (cacheGetCounter == 1)
                {
                    firstPreCachedResult = uResult;
                }
                else if (cacheGetCounter == 2)
                {
                    secondPreCachedResult = uResult;
                }
            };

            //act

            var firstGottenResult = await Repository.GetAsync(userId);

            await Task.Delay(_genericCachingRepository.ÑacheTimeoutMiliseconds);

            var secondGottenResult = await Repository.GetAsync(userId);

            //assert

            Assert.Multiple(() =>
            {
                Assert.That(firstPreCachedResult, Is.Null, "First precached value must be null");
                Assert.That(secondPreCachedResult, Is.Null, "Second precached value must be null");
                Assert.That(firstGottenResult, Is.EqualTo(secondGottenResult), $"{nameof(firstGottenResult)} must be same as {nameof(secondGottenResult)}");
            });
        }
        [Test]
        public async Task GetAsync_GetTwiseInARow_SecondValueGotFromCache()
        {
            //arrange

            Guid userId = Users[Random.Shared.Next(0, Users.Count)].Id;

            User? firstPreCachedResult = null;

            User? secondPreCachedResult = null;

            int cacheGetCounter = 0;

            _redis.OnGet += (key, result) =>
            {
                cacheGetCounter++;

                if (result is not User uResult)
                {
                    return;
                }

                if (cacheGetCounter == 1)
                {
                    firstPreCachedResult = uResult;
                }
                else if (cacheGetCounter == 2)
                {
                    secondPreCachedResult = uResult;
                }
            };

            //act

            var firstGottenResult = await Repository.GetAsync(userId);

            var secondGottenResult = await Repository.GetAsync(userId);

            //assert

            Assert.Multiple(() =>
            {
                Assert.That(firstPreCachedResult, Is.Null, "First precached value must be null");
                Assert.That(secondPreCachedResult, Is.Not.Null, "Second precached value must be not null");
                Assert.That(firstGottenResult, Is.EqualTo(secondGottenResult), $"{nameof(firstGottenResult)} must be same as {nameof(secondGottenResult)}");
                Assert.That(firstPreCachedResult, Is.Null, "First precached result has to be null");
                Assert.That(secondPreCachedResult, Is.EqualTo(secondGottenResult), "Second precached result has to be same as context value");
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _testDbContext.Dispose();
        }
    }
}