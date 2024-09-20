using Microsoft.EntityFrameworkCore;
using Repository.Tests.Models;
using Repository.Models;
using Repository.Contracts;

namespace Repository.Tests.GenericRepository
{
    public class UpdateAsyncTests
    {
        private IGenericRepository<User, Guid> _repository = null!;
        private TestDbContext _testDbContext = null!;

        private readonly List<User> _users = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Login = "Admin",
                Name = "Tom",
                Email = "Tom@gmail.com",
                Address = "Arizona"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Login = "User",
                Name = "Nick",
                Email = "Crazy@hotmail.com",
                Address = "Grece"
            }
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestDb", null).EnableServiceProviderCaching();
            _testDbContext = new(optionsBuilder.Options);
            _repository = new GenericRepository<User, Guid>(_testDbContext);

            _testDbContext.Users.AddRange(_users);
            _testDbContext.SaveChanges();
        }

        [Test]
        public async Task UpdateAsync_UpdatingContainedUser_ValueUpdated()
        {
            var userToUpdate = await _repository.GetAsync(_users.First().Id);

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

            var userAfterUpdate = await _repository.GetAsync(_users.First().Id);

            Assert.Multiple(() =>
            {
                Assert.That(userAfterUpdate, Is.EqualTo(userToUpdate));
                Assert.That(userAfterUpdate, Is.Not.EqualTo(userBeforeUpdate));
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
    }
}