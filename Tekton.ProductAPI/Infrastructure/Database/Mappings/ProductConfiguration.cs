using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tekton.ProductAPI.Domain.Entities;

namespace Tekton.ProductAPI.Infrastructure.Database.Mappings
{

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(product => product.ProductId);
            entityBuilder.ToTable("Products");
        }
    }
}
