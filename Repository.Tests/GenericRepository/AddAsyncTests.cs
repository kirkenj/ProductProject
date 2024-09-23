using Repository.Tests.Models;
using Repository.Contracts;
using Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Tests.GenericRepository
{
    public class AddAsyncTests
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
            var user3 = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@ya.ru",
                Login = "BBCReporter",
                Id = Users.First().Id
            };


            var func = async () => await _repository.AddAsync(user3);

            Assert.That(func, Throws.InvalidOperationException);
        }

        [Test]
        public async Task AddAsyncTests_ValueIsValid_AddsValue()
        {
            var user33 = new User
            {
                Address = "Zone 51",
                Name = "Natan",
                Email = "Natan228@uya.ru",
                Login = "BBCReportner",
                Id = Guid.NewGuid()
            };




            var u = await _testDbContext.Users.ToArrayAsync();

            //var q = _testDbContext.Entry(user33);

            var val = await _repository.GetAsync(user33.Id);

            if (val != null)
            {
                throw new InvalidOperationException();
            }

            await _testDbContext.Users.AddAsync(user33);

            await _testDbContext.SaveChangesAsync();

            var userFromContext = await _repository.GetAsync(user33.Id);

            Assert.That(userFromContext, Is.EqualTo(user33));
        }
    }
}