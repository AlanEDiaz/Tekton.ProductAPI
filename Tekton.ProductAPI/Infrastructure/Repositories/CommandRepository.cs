using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Infrastructure.Caching;
using System.Runtime.CompilerServices;
using System.Text.Json;
using LiteDB;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Tekton.ProductAPI.Infrastructure.Database;

namespace Tekton.ProductAPI.Infrastructure.Repositories
{
    public class CommandRepository:ICommandRepository
    {

        private readonly MainDbContext _dbContext;

        public CommandRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Push(object ACommand, CancellationToken cancellationToken = default)
        {
            try
            {
                Guid id;
                if (ACommand is Product product)
                {
                    id = product.ProductId;
                }
                else
                {
                    id = Guid.NewGuid();
                }

                var commandStore = new CommandStore
                {
                    ProductId = id,
                    Type = ACommand.GetType().Name,
                    Data = System.Text.Json.JsonSerializer.Serialize(ACommand),
                    CreatedAt = DateTime.Now
                };

                await _dbContext.Set<CommandStore>().AddAsync(commandStore, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on Entity Framework transaction: {ex.Message}");
            }
        }
    }
}
