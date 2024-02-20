using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tekton.ProductAPI.Domain.Entities;

namespace Tekton.ProductAPI.Infrastructure.Caching
{
    public interface IProductCache
    {
        Task<bool> TryGetValue(Guid key, out CachedProduct value);
        Task<bool> TryAdd(Guid key, CachedProduct value);
        Task<bool> TryRemove(Guid key, out CachedProduct value);
        IDictionary<Guid, CachedProduct> GetDictionary();
        Task<bool> TryUpdate(Guid key, CachedProduct oldValue, CachedProduct newValue);
        void ScheduleCacheCleanup();
    }
}
