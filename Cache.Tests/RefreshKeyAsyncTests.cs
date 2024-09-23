using Cache.Contracts;
using Cache.Models;
using Microsoft.Extensions.Options;

#pragma warning disable CS8625 // �������, ������ NULL, �� ����� ���� ������������ � ��������� ���, �� ����������� �������� NULL.

namespace Cache.Tests
{
    public class RefreshKeyAsyncTests
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

            _cache = new RedisCustomMemoryCache(options);
        }


        [Test]
        public async Task RefreshKeyAsync_KeyNotSet_ReturnsFalse()
        {
            var key = "hello world";

            var result = await _cache.RefreshKeyAsync(key, 300);
        
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RefreshKeyAsync_KeyIsEmptyString_ReturnsFalse()
        {
            var key = string.Empty;

            var result = await _cache.RefreshKeyAsync(key, 300);
        
            Assert.That(result, Is.False);
        }

        [Test]
        public void RefreshKeyAsync_KeyIsNull_ReturnsFalse()
        {

            var func = async () => await _cache.RefreshKeyAsync(null, 300);
        
            Assert.That(func, Throws.ArgumentException);
        }

        [Test]
        public async Task RefreshKeyAsync_KeyValueSetKeyRefreshed_ReturnsValue()
        {
            var value = Guid.NewGuid();

            var key = $"Key for {value}";

            await _cache.SetAsync(key, value, TimeSpan.FromMilliseconds(300));

            Thread.Sleep(200);

            Assert.That(await _cache.RefreshKeyAsync(key, 300), Is.True);

            Thread.Sleep(200);

            var valueFromCache = await _cache.GetAsync<Guid>(key);
        
            Assert.That(valueFromCache, Is.EqualTo(value));
        }

        [Test]
        public async Task RefreshKeyAsync_KeyValueSetKeyRefreshedWhenTimeIsUp_RefreshReturnsFalseGetValueReturnsNull()
        {
            var value = Guid.NewGuid();

            var key = $"Key for {value}";

            await _cache.SetAsync(key, value, TimeSpan.FromMilliseconds(300));

            Thread.Sleep(300);

            Assert.That(await _cache.RefreshKeyAsync(key, 300), Is.False);

            var valueFromCache = await _cache.GetAsync<Guid>(key);
        
            Assert.That(valueFromCache, Is.EqualTo(Guid.Empty));
        }
    }
}

#pragma warning restore CS8625 // �������, ������ NULL, �� ����� ���� ������������ � ��������� ���, �� ����������� �������� NULL.