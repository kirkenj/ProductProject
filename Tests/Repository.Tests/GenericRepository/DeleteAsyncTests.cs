using Repository.Models;
using Repository.Tests.Models;
using Repository.Tests.Models.TestBases;

namespace Repository.Tests.GenericRepository
{

    [TestFixture(typeof(GenericRepository<,>))]
    [TestFixture(typeof(GenericCachingRepository<,>))]
    [TestFixture(typeof(GenericFiltrableRepository<,,>))]
    [TestFixture(typeof(GenericFiltrableCachingRepository<,,>))]
    public class DeleteAsyncTests : GenericRepositoryTest
    {
        public DeleteAsyncTests(Type repType) : base(repType) { }


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
    }
}