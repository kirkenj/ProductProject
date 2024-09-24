using Microsoft.Extensions.Logging;
using Moq;
using Repository.Contracts;
using Repository.Models;
using Repository.Tests.Models;

namespace Repository.Tests.GenericCachingRepository
{
    public class CachingDeleteAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;
        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _customMemoryCache = TestConstants.GetEmptyReddis();

            _testDbContext = await TestConstants.GetDbContextAsync();

            var MockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _repository = new GenericCachingRepository<User, Guid>(_testDbContext, _customMemoryCache, MockLogger);
        }

        [SetUp]
        public void Setup()
        {
            _customMemoryCache.DropEvents();
            _customMemoryCache.ClearDb();
        }


        [Test]
        public void DeleteAsync_UserIsNull_ThrowsArgumentNullException()
        {
            var func = async () => await _repository.AddAsync(null);

            Assert.That(func, Throws.ArgumentNullException);
        }

        [Test]
        public async Task DeleteAsync_UserInDbSet_RemovesValue()
        {
            var user = Users.First();

            await _repository.DeleteAsync(user);

            var repUsers = await _repository.GetAllAsync();

            Assert.That(repUsers, Does.Not.Contain(user));
        }

        [Test]
        public void DeleteAsync_UserNotInDbSet_ThrowsArgumentException()
        {
            var user = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.ru",
                Login = "BBCReporter",
                Id = Guid.NewGuid()
            };

            var func = async () => await _repository.DeleteAsync(user);


            Assert.That(func, Throws.Exception);
        }


        [Test]
        public async Task DeleteAsync_UserInDbSetGetAndDelete_RemovesFromDbAndCache()
        {
            //arrange
            var user = Users[Random.Shared.Next(Users.Count)];

            string? keyToGetCachedUser = null;

            _customMemoryCache.OnSet += (key, value, span) =>
            {
                if (value.Equals(user))
                {
                    keyToGetCachedUser = key;
                }
            };

            //act
            var userFromContext = await _repository.GetAsync(user.Id);

            var userFromCache = await _customMemoryCache.GetAsync<User>(keyToGetCachedUser);

            await _repository.DeleteAsync(userFromContext);

            var attemptToGetuserFromContext = await _repository.GetAsync(user.Id);

            var attemptToGetuserFromCache = await _customMemoryCache.GetAsync<User>(keyToGetCachedUser);

            //asssert
            Assert.Multiple(() =>
            {
                Assert.That(userFromCache, Is.Not.Null);

                Assert.That(attemptToGetuserFromContext, Is.Null);

                Assert.That(attemptToGetuserFromCache, Is.Null);
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _testDbContext.Dispose();
        }
    }
}