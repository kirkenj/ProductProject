using Application.Contracts.Persistence;


namespace Persistence.Tests.UserRepository
{
    public class Tests
    {
        private IUserRepository _userRepository;


        [SetUp]
        public void Setup()
        {
            // var options = new DbContextOptionsBuilder<AuthDbContext>()
            //.UseInMemoryDatabase(databaseName: "AuthDbContext")
            //.Options;

            // var dbc = new AuthDbContext(options);

            // //dbc.Set<User>().Add(new User() { Id = Guid.NewGuid(), Email = "kirkenj@bk.ru", RoleID = });
            // dbc.SaveChanges();

            // _userRepository = new Repositories.UserRepository(dbc);
        }

        [Test]
        public async Task User_Repository_empty()
        {
            //    var users = await _userRepository.GetAllAsync();
            //    Assert.That(users, Is.Empty);
        }
    }
}