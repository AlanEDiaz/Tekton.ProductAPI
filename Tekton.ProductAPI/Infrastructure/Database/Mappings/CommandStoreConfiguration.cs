using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tekton.ProductAPI.Domain.Entities;

namespace Tekton.ProductAPI.Infrastructure.Database.Mappings
{
    public class CommandStoreConfiguration : IEntityTypeConfiguration<CommandStore>
    {
        public void Configure(EntityTypeBuilder<CommandStore> AEntityBuilder)
            => AEntityBuilder.ToTable("CommandStore");
    }
}
