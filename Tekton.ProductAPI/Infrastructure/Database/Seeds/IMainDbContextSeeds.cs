using Microsoft.EntityFrameworkCore;

namespace Tekton.ProductAPI.Infrastructure.Database.Seeds
{
    public interface IMainDbContextSeeds
    {
        void Seed(ModelBuilder modelBuilder);
    }
}
