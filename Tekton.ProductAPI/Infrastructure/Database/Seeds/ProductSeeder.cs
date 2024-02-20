using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Tekton.ProductAPI.Domain.Entities;

namespace Tekton.ProductAPI.Infrastructure.Database.Seeds
{
    public class ProductSeeder : IMainDbContextSeeds
    {
        public void Seed(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Product>().HasData(CreateProduct());

        private static IEnumerable<Product> CreateProduct()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId=Guid.NewGuid(),
                    Name="ProductSeed1",
                    Stock=12,
                    Description="test product1",
                    Price=234,
                    Discount=0,
                    CreatedOn=DateTime.Now,
                    UpdatedOn=null
    },
                new Product
                {
                   ProductId=Guid.NewGuid(),
                    Name="ProductSeed2",
                    Stock=14,
                    Description="test product2",
                    Price=23,
                    Discount=0,
                    CreatedOn=DateTime.Now,
                    UpdatedOn=null
                }
            };
        }
    }
}
