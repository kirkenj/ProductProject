using Cache.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Cache.Tests
{
    public class ConstructorTests
    {

        [Test]
        public void Constructor_NullUriNullContainerName_ThrowsArgumentNullException()
        {
            //arrange
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            CustomCacheOptions customCacheOptions = new()
            {
                ConnectionUri = null
            };
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

            IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

            //act
            var instanseInitDelefate = () => new RedisCustomMemoryCache(options);

            //assert
            Assert.That(instanseInitDelefate, Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_IncorrectUri_ThrowsRedisConnectionException()
        {
            //arrange
            CustomCacheOptions customCacheOptions = new()
            {
                ConnectionUri = "null"
            };

            IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

            //act
            var instanseInitDelefate = () => new RedisCustomMemoryCache(options);

            //assert
            Assert.That(instanseInitDelefate, Throws.TypeOf<RedisConnectionException>());
        }

        [Test]
        public void Constructor_CorrectUri_ReturnsValue()
        {
            //arrange
            CustomCacheOptions customCacheOptions = new()
            {
                ConnectionUri = "localhost:3330"
            };

            IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

            //act
            var instanseInitDelefate = new RedisCustomMemoryCache(options);

            //assert
            Assert.Pass();
        }
    }
}