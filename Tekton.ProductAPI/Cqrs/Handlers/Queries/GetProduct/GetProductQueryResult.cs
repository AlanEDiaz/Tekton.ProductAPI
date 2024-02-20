using System;

namespace Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct
{
    public class GetProductQueryResult
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int StatusName { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
