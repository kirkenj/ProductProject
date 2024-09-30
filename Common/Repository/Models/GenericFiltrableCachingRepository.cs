﻿using Cache.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using System.Text.Json;

namespace Repository.Models
{
    public class GenericFiltrableCachingRepository<T, TIdType, TFilter> :
        GenericCachingRepository<T, TIdType>,
        IGenericCachingRepository<T, TIdType>,
        IGenericFiltrableRepository<T, TIdType, TFilter>
        where T : class, IIdObject<TIdType> where TIdType : struct
    {
        public GenericFiltrableCachingRepository(DbContext dbContext, ICustomMemoryCache customMemoryCache, ILogger<GenericCachingRepository<T, TIdType>> logger, Func<IQueryable<T>, TFilter, IQueryable<T>> getFilteredSetDelegate)
            : base(dbContext, customMemoryCache, logger)
        {
            GetFilteredSetDelegate = getFilteredSetDelegate;
        }

        private readonly Func<IQueryable<T>, TFilter, IQueryable<T>> GetFilteredSetDelegate;

        public virtual async Task<T?> GetAsync(TFilter filter)
        {
            var key = JsonSerializer.Serialize(filter) + "First";

            _logger.LogInformation($"Got request: {key}");

            var cacheResult = await CustomMemoryCache.GetAsync<T>(key);

            if (cacheResult != null)
            {
                _logger.LogInformation($"Request {key}. Found in cache");
                return cacheResult;
            }

            _logger.LogInformation($"Request {key}. Requesting the database");

            var repResult = await GetFilteredSetDelegate(DbSet, filter).FirstOrDefaultAsync();

            if (repResult != null)
            {
                await SetCacheAsync(key, repResult);
            }

            return repResult;
        }


        public virtual async Task<IReadOnlyCollection<T>> GetPageContent(TFilter filter, int? page = default, int? pageSize = default)
        {
            var key = JsonSerializer.Serialize(filter) + $"page: {page}, pageSize: {pageSize}";

            _logger.LogInformation($"Got request: {key}");

            var result = await CustomMemoryCache.GetAsync<IReadOnlyCollection<T>>(key);

            if (result != null)
            {
                _logger.LogInformation($"Request {key}. Found in cache");
                return result;
            }

            _logger.LogInformation($"Request {key}. Requesting the database");
            result = await GetPageContent(GetFilteredSetDelegate(DbSet, filter), page, pageSize).ToArrayAsync();

            var tasks = result.Select(item => SetCacheAsync(string.Format(CacheKeyFormatToAccessSingleViaId, item.Id), item))
                .Append(SetCacheAsync(key, result));

            await Task.WhenAll(tasks);

            return result;
        }
    }
}