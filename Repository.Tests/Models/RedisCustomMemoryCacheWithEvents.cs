﻿using Cache.Models;
using Microsoft.Extensions.Options;

namespace Repository.Tests.Models
{
    public class RedisCustomMemoryCacheWithEvents : RedisCustomMemoryCache
    {
        public RedisCustomMemoryCacheWithEvents(IOptions<CustomCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }
        public RedisCustomMemoryCacheWithEvents(CustomCacheOptions optionsAccessor) : base(optionsAccessor)
        {
        }

        public delegate void OnGetHandler(string key, object? result);

        public event OnGetHandler? OnGet;

        public override async Task<T?> GetAsync<T>(string key) where T : default
        {
            var result = await base.GetAsync<T>(key);
            OnGet?.Invoke(key, result);
            return result;
        }


        public delegate void OnRefreshKeyHandler(string key, double millisecondsToExpire);

        public event OnRefreshKeyHandler? OnRefreshKey;

        public override Task<bool> RefreshKeyAsync(string key, double millisecondsToExpire)
        {
            var result = base.RefreshKeyAsync(key, millisecondsToExpire);
            OnRefreshKey?.Invoke(key, millisecondsToExpire);
            return result;
        }

        public delegate void OnRemoveHandler(string key);

        public event OnRemoveHandler? OnRemove;

        public override Task RemoveAsync(string key)
        {
            var result = base.RemoveAsync(key);
            OnRemove?.Invoke(key);
            return result;
        }

        public delegate void OnSetHandler(string key, object? value, TimeSpan expirity);

        public event OnSetHandler? OnSet;

        public override Task SetAsync<T>(string key, T value, TimeSpan expirity)
        {
            var result = base.SetAsync(key, value, expirity);
            OnSet?.Invoke(key, value, expirity);
            return result;
        }

        public void DropEvents()
        {
            OnGet = null;
            OnSet = null;
            OnRemove = null;
        }

        public void ClearDb()
        {
            _implementation.Execute("FLUSHALL");
        }
    }
}
