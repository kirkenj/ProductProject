using Repository.Tests.Models;
using Repository.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Repository.Tests.GenericFiltrableCachingRepository
{
    public class CachingUpdateAsyncTests
    {
        private List<User> Users => _testDbContext.Users.ToList();
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
        public void SetUp()
        {
            _customMemoryCache.ClearDb();
            _customMemoryCache.DropEvents();
        }

        [Test]
        public async Task UpdateAsync_UpdatingContainedUser_ValueUpdatedAndAddedToCache()
        {
            var userToUpdate = Users[Random.Shared.Next(Users.Count)];

            User? userAddedToCache = null;

            _customMemoryCache.OnSet += (key, value, offset) =>
            {
                if (value is User uVal && uVal.Id == userToUpdate.Id)
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

            var userAfterUpdate = await _repository.GetAsync(userToUpdate.Id) ?? throw new ArgumentException();

            Assert.Multiple(() =>
            {
                Assert.That(userAfterUpdate, Is.Not.EqualTo(userBeforeUpdate), $"Is equal: {userAfterUpdate.Equals(userBeforeUpdate)}");
                Assert.That(userAfterUpdate, Is.EqualTo(userToUpdate));
                Assert.That(userAfterUpdate, Is.EqualTo(userAddedToCache));
            });
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