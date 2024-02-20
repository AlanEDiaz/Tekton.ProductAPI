using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tekton.ProductAPI.Models;
using Tekton.ProductAPI.Services;

namespace Tekton.ProductAPI.Controllers
{
    /// <summary>
    /// Controller responsible for handling product-related operations.
    /// </summary>
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        [Route("/GetAllProducts")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        [Route("/GetProductById/{id}")]
        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product data to create.</param>
        [Route("/CreateProduct")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProduct(CreateProductDto product)
        {
            var productId = await _productService.CreateProductAsync(product);
            return Ok(productId);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="updateProductDto">The updated product data.</param>
        [Route("/UpdateProduct")]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var result = await _productService.UpdateProductAsync(updateProductDto);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
