using System.Threading.Tasks;
using System.Threading;
using System;

namespace Tekton.ProductAPI.Infrastructure.Repositories
{
    public interface ICommandRepository
    {
        Task Push(object ACommand, CancellationToken ACancellationToken = default);
    }
}
