using System;

namespace Tekton.ProductAPI.Models
{
    /// <summary>
    /// Represents a product data transfer object (DTO) used for API responses.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status name of the product.
        /// </summary>
        public int StatusName { get; set; }

        /// <summary>
        /// Gets or sets the available stock quantity of the product.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the product.
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// Gets or sets the final price of the product after applying discounts.
        /// </summary>
        public decimal FinalPrice { get; set; }
    }
}
