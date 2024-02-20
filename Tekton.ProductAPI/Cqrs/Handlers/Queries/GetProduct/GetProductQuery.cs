using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Models;
using System;

namespace Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct
{
    public class GetProductQuery : IRequest<GetProductQueryResult>
    {
        public Guid ProductId { get; }

        public GetProductQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
