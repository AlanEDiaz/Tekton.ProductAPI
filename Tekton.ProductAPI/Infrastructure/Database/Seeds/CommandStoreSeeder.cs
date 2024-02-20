using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Tekton.ProductAPI.Domain.Entities;

namespace Tekton.ProductAPI.Infrastructure.Database.Seeds
{
    public class CommandStoreSeeder : IMainDbContextSeeds
    {
        public void Seed(ModelBuilder modelBuilder)
            => modelBuilder.Entity<CommandStore>().HasData(CreateCommandStore());

        private static IEnumerable<CommandStore> CreateCommandStore()
        {
            return new List<CommandStore>
            {
                new CommandStore
                {
                    id = 1,
                    ProductId = Guid.NewGuid(),
                    Type = "Create",
                    Data = "test",
                    CreatedAt = DateTime.Now
                },
                new CommandStore
                {
                    id = 2,
                    ProductId = Guid.NewGuid(),
                    Type = "Update",
                    Data = "test",
                    CreatedAt = DateTime.Now
                }
            };
        }
    }
}
