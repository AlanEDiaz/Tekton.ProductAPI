using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Infrastructure.Caching;
using Tekton.ProductAPI.Infrastructure.Database;
using Tekton.ProductAPI.Infrastructure.Repositories;

namespace Tekton.ProductAPI.Tests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private IProductCache _cache;

        [SetUp]
        public void SetUp()
        {

            _cache = A.Fake<IProductCache>();

        }

        [Test]
        public async Task GetById_ProductExists_ReturnsProduct()
        {
            using (var dbContext = CreateDbContext())
            {
                // Arrange
                var productId = Guid.NewGuid();
                var product = new Product { ProductId = productId, Name = "Test Product" };
                await dbContext.Product.AddAsync(product);
                await dbContext.SaveChangesAsync();
                var repository = new ProductRepository(dbContext, _cache);

                // Act
                var result = await repository.GetById(productId);

                // Assert
                result.Should().NotBeNull();
                result.ProductId.Should().Be(productId);
                result.Name.Should().Be("Test Product");
            }
        }
        [Test]
        public async Task GetAll_WhenProductsExist_ReturnsAllProducts()
        {
            using (var dbContext = CreateDbContext())
            {
                // Arrange

                var repository = new ProductRepository(dbContext, _cache);

                var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Product 1" },
                new Product { ProductId = Guid.NewGuid(), Name = "Product 2" }
            };
                if (dbContext.Product.FirstOrDefault()!=null)
                {
                    dbContext.Product.Remove(dbContext.Product.FirstOrDefault());
                }
                dbContext.Product.AddRange(products);
                dbContext.SaveChanges();

                // Act
                var result = await repository.GetAll();

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(2);
            }
        }

        [Test]
        public async Task Add_ValidProduct_ReturnsProductId()
        {
            using (var dbContext = CreateDbContext())
            {
                // Arrange
                var repository = new ProductRepository(dbContext, _cache);

                var product = new Product { ProductId = Guid.NewGuid(), Name = "New Product" };

                // Act
                var result = await repository.Add(product);

                // Assert
                result.Should().Be(product.ProductId);
            }
        }

        [Test]
        public async Task UpdateProductAsync_ExistingProduct_UpdatesProduct()
        {
            using (var dbContext = CreateDbContext())
            {


                // Arrange
                var repository = new ProductRepository(dbContext, _cache);
                var existingProduct = new Product { ProductId = Guid.NewGuid(), Name = "Existing Product" };
                dbContext.Product.Add(existingProduct);
                dbContext.SaveChanges();

                var updatedProduct = new Product
                {
                    ProductId = existingProduct.ProductId,
                    Name = "Updated Product",
                    Description = "Updated Description",
                    Price = 20.5m,
                    Stock = 50,
                    UpdatedOn = DateTime.Now
                };

                // Act
                var result = await repository.UpdateProductAsync(updatedProduct);

                // Assert
                result.Should().BeTrue();
                var productInDb = dbContext.Product.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
                productInDb.Should().NotBeNull();
                productInDb.Name.Should().Be(updatedProduct.Name);
                productInDb.Description.Should().Be(updatedProduct.Description);
                productInDb.Price.Should().Be(updatedProduct.Price);
                productInDb.Stock.Should().Be(updatedProduct.Stock);
            }
        }

        [Test]
        public async Task AddProductCache_ValidProduct_AddsProductToCache()
        {
            using (var dbContext = CreateDbContext())
            {
                // Arrange
                var repository = new ProductRepository(dbContext, _cache);
                
                var product = new Product
                {
                    ProductId = Guid.NewGuid(),
                    Name = "New Product",
                    Stock = 100,
                    Description = "Description",
                    Price = 10.5m,
                    Discount = 5,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = null
                };

                // Act
                await repository.AddProductCache(product);

                // Assert
                A.CallTo(() => _cache.TryAdd(product.ProductId, A<CachedProduct>._)).MustHaveHappenedOnceExactly();
            }
        }
        private MainDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MainDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new MainDbContext(options);
        }
    }
}
