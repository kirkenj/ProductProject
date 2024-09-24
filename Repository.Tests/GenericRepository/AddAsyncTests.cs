using Repository.Tests.Models;
using Repository.Contracts;
using Repository.Models;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Repository.Tests.GenericRepository
{
    [TestFixture(typeof(GenericRepository<,>))]
    [TestFixture(typeof(GenericCachingRepository<,>))]
    [TestFixture(typeof(GenericFiltrableRepository<,,>))]
    [TestFixture(typeof(GenericFiltrableCachingRepository<,,>))]
    public class AddAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;
        private List<User> Users => _testDbContext.Users.ToList();

        public AddAsyncTests(Type repType)
        {
            var contextTask = TestConstants.GetDbContextAsync();
            contextTask.Wait();
            _testDbContext = contextTask.Result;

            if (repType == typeof(GenericRepository<,>))
            {
                _repository = new GenericRepository<User, Guid>(_testDbContext);
            }
            else if (repType == typeof(GenericCachingRepository<,>))
            {
                var mockLogger = Mock.Of<ILogger<GenericCachingRepository<User, Guid>>>();

                _repository = new GenericCachingRepository<User, Guid>(_testDbContext, TestConstants.GetReddis(), mockLogger);
            }
            else if (repType == typeof(GenericFiltrableRepository<,,>))
            {
                _repository = new GenericFiltrableRepository<User, Guid, UserFilter>(_testDbContext, UserFilter.GetFilteredSet);
            }
            else if (repType == typeof(GenericFiltrableCachingRepository<,,>))
            {
                var mockLogger = Mock.Of<ILogger<GenericFiltrableCachingRepository<User, Guid, UserFilter>>>();

                _repository = new GenericFiltrableCachingRepository<User, Guid, UserFilter>(_testDbContext, TestConstants.GetReddis(), mockLogger, UserFilter.GetFilteredSet);
            }
        }

        public void AddAsync_UserIsNull_ThrowsArgumentNullException()
        {
            var func = async () => await _repository.AddAsync(null);

            Assert.That(func, Throws.ArgumentNullException);
        }

        [Test]
        public void AddAsyncTests_UserAlreadyInDbSet_ThrowsArgumentException()
        {
            var func = async () => await _repository.AddAsync(Users.First());

            Assert.That(func, Throws.ArgumentException);
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
                Id = Users.First().Id
            };

            var func = async () => await _repository.AddAsync(user);

            Assert.That(func, Throws.InvalidOperationException);
        }

        [Test]
        public async Task AddAsyncTests_ValueIsValid_AddsValue()
        {
            var user = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@uya.ru",
                Login = "BBCReportner",
                Id = Guid.NewGuid()
            };

            await _repository.AddAsync(user);

            var userFromContext = await _repository.GetAsync(user.Id);

            Assert.That(userFromContext, Is.EqualTo(user));
        }
    }
}