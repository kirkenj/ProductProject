using Cache.Contracts;
using Cache.Models;
using Microsoft.Extensions.Options;

namespace Cache.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_NullUriNullContainerName_ThrowsArgumentNullException()
        {
            //arrange
            CustomCacheOptions customCacheOptions = new()
            {

                ConnectionUri = null,
                DockerContainerName = null

            };

            IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

            //act
            var instanseInitDelefate = () => new RedisAsMemoryCache(options);

            //assert
            Assert.That(instanseInitDelefate, Throws.ArgumentNullException);
        }
    }
}