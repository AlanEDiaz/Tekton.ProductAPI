using System.Collections.Concurrent;
using System;
using Tekton.ProductAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tekton.ProductAPI.Infrastructure.Caching
{
    public class ProductCache : IProductCache
    {
        private readonly ConcurrentDictionary<Guid, CachedProduct> _cache = new ConcurrentDictionary<Guid, CachedProduct>();

        public Task<bool> TryGetValue(Guid key, out CachedProduct value)
        {
            return Task.FromResult(_cache.TryGetValue(key, out value));
        }

        public Task<bool> TryAdd(Guid key, CachedProduct value)
        {
            return Task.FromResult(_cache.TryAdd(key, value));
        }
        public Task<bool> TryUpdate(Guid key, CachedProduct oldValue,CachedProduct newValue)
        {
            return Task.FromResult(_cache.TryUpdate(key, newValue,oldValue ));
        }

        public Task<bool> TryRemove(Guid key, out CachedProduct value)
        {
            return Task.FromResult(_cache.TryRemove(key, out value));
        }
        public IDictionary<Guid, CachedProduct> GetDictionary()
        {
            return _cache;
        }
        public void ScheduleCacheCleanup()
        {
            var cleanupInterval = TimeSpan.FromMinutes(5);
            Task.Run(async () =>
            {
                await Task.Delay(cleanupInterval);
                CleanupExpiredCacheEntries();
            });
        }

        private void CleanupExpiredCacheEntries()
        {
            var currentTime = DateTime.Now;
            var cacheDictionary = GetDictionary();
            foreach (var kvp in cacheDictionary)
            {
                if ((currentTime - kvp.Value.LastAccessed).TotalMinutes > 5)
                {
                    _cache.TryRemove(kvp.Key, out _);
                }
            }
        }
    }
}
