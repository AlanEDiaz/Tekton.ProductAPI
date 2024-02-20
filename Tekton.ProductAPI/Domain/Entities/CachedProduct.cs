using System;

namespace Tekton.ProductAPI.Domain.Entities
{
    public class CachedProduct:Product
    {
        public DateTime LastAccessed { get; set; }
        public int StatusName { get; set; }

    }
}
