using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Repository.Tests.GenericCachingRepository
{
    public class CachingAddAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private RedisCustomMemoryCacheWithEvents _customMemoryCache = null!;
        private ILogger<GenericCachingRepository<User,Guid>> _logger = null!;
        private List<User> Users => _testDbContext.Users.ToList();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _customMemoryCache = TestConstants.GetReddis();
           
            _testDbContext = await TestConstants.GetDbContextAsync();

            _logger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

            _repository = new GenericCachingRepository<User, Guid>(_testDbContext, _customMemoryCache, _logger);
        }

        [SetUp]
        public void SetUp()
        {
            _customMemoryCache.DropEvents();
            _customMemoryCache.ClearDb();
        }

        [Test]
        public void AddAsync_UserIsNull_ThrowsArgumentNullException()
        {
            var func = async () => await _repository.AddAsync(null);

            Assert.That(func, Throws.ArgumentNullException);
        }

        [Test]
        public void AddAsyncTests_UserAlreadyInDbSet_ThrowsException()
        {
            var func = async () => await _repository.AddAsync(Users.First());

            Assert.That(func, Throws.Exception);
        }

        [Test]
        public void AddAsyncTests_IDIsTaken_ThrowsInvalidOperationException()
        {
            var user = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.ru",
                Login = "BBCReporter",
                Id = _testDbContext.Users.First().Id
            };

            var func = async () => await _repository.AddAsync(user);

            Assert.That(func, Throws.InvalidOperationException);
        }   

        [Test]
        public async Task AddAsyncTests_ValueIsValid_AddsValueToContextAndCache()
        {
            var user223 = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.rru",
                Login = "BBCReporter3",
            };

            User? userFromCache = null;

            _customMemoryCache.OnSet += (key, value, span) =>
            {
                if (value is User uVal && uVal.Equals(user223))
                {
                    userFromCache = uVal;
                }
            };

            await _repository.AddAsync(user223);

            var userFromContext = await _repository.GetAsync(user223.Id);

            Assert.That(userFromContext, Is.EqualTo(user223));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _testDbContext.Dispose();
        }
    }
}