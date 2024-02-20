using MediatR;
using System;

namespace Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

    }
}
