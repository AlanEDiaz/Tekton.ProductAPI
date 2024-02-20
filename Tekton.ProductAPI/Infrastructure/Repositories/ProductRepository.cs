using LiteDB;
using System.Threading.Tasks;
using System;
using Tekton.ProductAPI.Domain.Entities;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tekton.ProductAPI.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore;
using Tekton.ProductAPI.Infrastructure.Database;

namespace Tekton.ProductAPI.Infrastructure.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly MainDbContext _dbContext;
        private readonly IProductCache _cache;

        public ProductRepository(MainDbContext dbContext, IProductCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _dbContext.Set<Product>().FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbContext.Set<Product>().ToListAsync();
        }

        public async Task<Guid> Add(Product product)
        {
            await _dbContext.Set<Product>().AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existingProduct = await _dbContext.Set<Product>().FindAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.ProductId} not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task AddProductCache(Product product)
        {
            if (product != null)
            {
                var cachedProductToAdd = new CachedProduct
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Stock = product.Stock,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn,
                    LastAccessed = DateTime.Now,
                    StatusName = 1
                };

                await _cache.TryAdd(product.ProductId, cachedProductToAdd);
            }
        }
    }
    
}
