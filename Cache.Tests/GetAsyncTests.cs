using Cache.Contracts;
using Cache.Models;
using Cache.Tests.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

namespace Cache.Tests
{
    public class GetAsyncTests
    {
        private ICustomMemoryCache _cache;

        [SetUp]
        public void Setup()
        {
            CustomCacheOptions customCacheOptions = new()
            {
                ConnectionUri = "localhost:3330"
            };

            IOptions<CustomCacheOptions> options = Options.Create(customCacheOptions);

            _cache = new RedisAsMemoryCache(options);
        }

        [Test]
        public void GetAsync_KeyNull_ThrowsArgumentNullException()
        {
            //act
            var func = async () => await _cache.GetAsync<object>(null);

            //assert
            Assert.That(func, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetAsync_KeyEmpty_ThrowsArgumentNullException()
        {
            //act
            var func = async () => await _cache.GetAsync<object>(string.Empty);

            //assert
            Assert.That(func, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetAsync_KeyWhitespace_ThrowsArgumentNullException()
        {
            //act
            var func = async () => await _cache.GetAsync<object>(" ");

            //assert
            Assert.That(func, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task GetAsync_KeyValidValueNotSet_ReturnsNull()
        {
            //arrange
            var key = Guid.NewGuid().ToString();

            //act
            var result = await _cache.GetAsync<object>(key);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAsync_KeyValidValueValidOffsetValidReturnTypeInvalid_ThrowsJsonException()
        {
            //arrange
            var key = Guid.NewGuid().ToString();

            var value = new Human { Name = "Rick", Surname = "Sanckezz" };

            //act
            await _cache.SetAsync(key, value, DateTimeOffset.Now.AddSeconds(3));

            Exception? thrownException = null;

            try
            {
                await _cache.GetAsync<double>(key);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            //assert
            Assert.That(thrownException is JsonException);
        }

        [Test]
        public async Task GetAsync_ValidArgumentsTimeIsOut_ReturnsNull()
        {
            //arrange
            var key = Guid.NewGuid().ToString();

            var value = new Human { Name = "Rick", Surname = "Sanckezz" };

            int milisecs = 120;

            //act
            await _cache.SetAsync(key, value, DateTimeOffset.Now.AddMilliseconds(milisecs));

            Thread.Sleep(milisecs);

            var result = await _cache.GetAsync<Human>(key);

            //assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAsync_ValidArgumentsTimeIsNotOut_ReturnsEqualValue()
        {
            //arrange
            var key = Guid.NewGuid().ToString();

            var value = new Human { Name = "Rick", Surname = "Sanckezz" };

            //act
            await _cache.SetAsync(key, value, DateTimeOffset.Now.AddSeconds(3));

            var result = await _cache.GetAsync<Human>(key);

            //assert
            Assert.That(value, Is.EqualTo(result));
        }

        [Test]
        public async Task GetAsync_ValidArgumentsTimeIsBoundaryNotOut_ReturnsEqualValue()
        {
            //arrange
            var key = Guid.NewGuid().ToString();

            var value = new Human { Name = "Rick", Surname = "Sanckezz" };
            
            int milisecs = 6000;

            int inaccuracy = 1200;

            var timeToSleep = milisecs - inaccuracy;

            //act
            _ = Task.Run(async () => await _cache.SetAsync(key, value, DateTimeOffset.Now.AddMilliseconds(milisecs)));

            Thread.Sleep(timeToSleep);

            var result = await _cache.GetAsync<Human>(key);

            //assert
            Assert.That(result, Is.EqualTo(value));
        }
    }
}

#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.