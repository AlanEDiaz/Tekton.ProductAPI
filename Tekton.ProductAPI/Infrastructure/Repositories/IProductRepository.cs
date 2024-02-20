using System.Threading.Tasks;
using System;
using Tekton.ProductAPI.Domain.Entities;
using System.Collections.Generic;

namespace Tekton.ProductAPI.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(Guid id);
        Task<Guid> Add(Product product);
        Task<IEnumerable<Product>> GetAll();
        Task<bool> UpdateProductAsync(Product product);
        Task AddProductCache(Product product);
    }
}
