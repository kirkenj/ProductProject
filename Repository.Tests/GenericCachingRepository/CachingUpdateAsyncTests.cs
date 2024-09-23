using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;
using Cache.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Repository.Tests.GenericCachingRepository
{
    public class CachingUpdateAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;
        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task Setup()
        {
            var redis = TestConstants.GetReddis();

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
        public async Task UpdateAsync_UpdatingContainedUser_ValueUpdatedAndAddedToCache()
        {
            var id = Users[Random.Shared.Next(Users.Count)].Id;

            var userToUpdate = await _repository.GetAsync(id);

            if (userToUpdate == null)
            {
                Assert.Fail();
                return;
            }

            User? userAddedToCache = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal && uVal.Id == id)
                {
                    userAddedToCache = uVal;
                }
            };

            var userBeforeUpdate = new User
            {
                Id = userToUpdate.Id,
                Name = userToUpdate.Name,
                Email = userToUpdate.Email,
                Address = userToUpdate.Address,
                Login = userToUpdate.Login
            };

            userToUpdate.Name = Guid.NewGuid().ToString();

            await _repository.UpdateAsync(userToUpdate);

            var userAfterUpdate = await _repository.GetAsync(id);


            Assert.That(userAfterUpdate, Is.Not.EqualTo(userBeforeUpdate), $"Is equal: {userAfterUpdate.Equals(userBeforeUpdate)}");
            Assert.That(userAfterUpdate, Is.EqualTo(userToUpdate));
            Assert.That(userAfterUpdate, Is.EqualTo(userAddedToCache));
        }


        [Test]
        public void UpdateAsync_UpdatingNotContainedUser_ThrowsException()
        {
            var userToUpdate = new User
            {
                Id = Guid.NewGuid(),
                Name = "userToUpdate.Name",
                Email = "userToUpdate.Email",
                Address = "userToUpdate.Address",
                Login = "userToUpdate.Login"
            };


            var func = async () => await _repository.UpdateAsync(userToUpdate);

            Assert.That(func, Throws.Exception);
        }
    

        [Test]
        public void UpdateAsync_UpdatingNull_ThrowsException()
        {
            User? userToUpdate = null;

            var func = async () => await _repository.UpdateAsync(userToUpdate);

            Assert.That(func, Throws.Exception);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testDbContext.Dispose();
        }
    }
}