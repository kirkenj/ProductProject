using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class UpdateAsyncTests
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
        public async Task UpdateAsync_UpdatingContainedUser_ValueUpdated()
        {
            var id = _testDbContext.Users.First().Id;

            var userToUpdate = await _repository.GetAsync(id);

            if (userToUpdate == null)
            {
                Assert.Fail();
                return;
            }

            var userBeforeUpdate = new User
            {
                Id = userToUpdate.Id,
                Name = userToUpdate.Name,
                Email = userToUpdate.Email,
                Address = userToUpdate.Address,
                Login = userToUpdate.Login
            };

            userToUpdate.Name = "Updated name";

            await _repository.UpdateAsync(userToUpdate);

            var userAfterUpdate = await _repository.GetAsync(id);

            Assert.That(userAfterUpdate, Is.Not.EqualTo(userBeforeUpdate));
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