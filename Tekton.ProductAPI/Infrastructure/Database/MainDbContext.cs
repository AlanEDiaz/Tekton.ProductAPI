using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Tekton.ProductAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Tekton.ProductAPI.Infrastructure.Database.Seeds;

namespace Tekton.ProductAPI.Infrastructure.Database
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        public MainDbContext() { }
        public virtual DbSet<Product> Product { get; set; }


        public virtual DbSet<CommandStore> CommandStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyConfiguration(modelBuilder);

            new ProductSeeder().Seed(modelBuilder);
            new CommandStoreSeeder().Seed(modelBuilder);
        }

        private static void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
