using System;
using System.ComponentModel.DataAnnotations;

namespace Tekton.ProductAPI.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) used to update product information.
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the updated name of the product.
        /// </summary>
        [MaxLength(100, ErrorMessage = "Name must be at most 100 characters long")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the updated description of the product.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters long")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the updated price of the product.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the updated stock quantity of the product.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer value")]
        public int? Stock { get; set; }

        /// <summary>
        /// Gets or sets the updated discount percentage applied to the product.
        /// </summary>
        [Range(0, 100, ErrorMessage = "Discount must be a number between 0-100 and non-negative decimal value")]
        public decimal? Discount { get; set; }
    }
}
