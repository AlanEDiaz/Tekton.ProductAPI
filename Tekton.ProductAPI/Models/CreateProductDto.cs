using System.ComponentModel.DataAnnotations;

namespace Tekton.ProductAPI.Models
{
    /// <summary>
    /// Represents the data transfer object (DTO) for creating a new product.
    /// </summary>
    public class CreateProductDto
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name must be at most 100 characters long.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// </summary>
        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer value.")]
        public int Stock { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters long.")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the product.
        /// </summary>
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }
    }
}
