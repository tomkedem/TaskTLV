using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs;
using ProductAPI.Entities;
using ProductAPI.Services;
using Microsoft.Extensions.Logging;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>Returns all products in the system.</returns>
        [Authorize] // Ensure the user is authorized
        [HttpGet]  // Define this as a GET endpoint
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("Retrieving all products");

            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                _logger.LogWarning("No products found");
                return NotFound("No products available");
            }

            return Ok(products);
        }
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Returns the product details if found; otherwise, returns a 404 error.</returns>
        [Authorize(Roles = "Viewer,Editor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found", id);
                return NotFound();
            }

            var productDto = new ProductDto
            {
                ProductId = product.ProductId,
                InStock = product.InStock,
                ArrivalDate = product.ArrivalDate,
                ProductName = product.ProductName // Get ProductName from ProductDetails
            };

            _logger.LogInformation("Product retrieved successfully with ID: {ProductId}", id);
            return Ok(productDto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="updatedProductDto">The product details to update.</param>
        /// <returns>Returns 204 No Content if the update is successful; otherwise, returns 404 or 500 on error.</returns>
        [Authorize(Roles = "Editor")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto updatedProductDto)
        {
            // Validate the incoming product update data using model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for Product ID: {ProductId}. Returning BadRequest.", updatedProductDto.ProductId);
                return BadRequest(ModelState); // Return a BadRequest if the validation fails
            }

            _logger.LogInformation("Attempting to update product with ID: {ProductId}", updatedProductDto.ProductId);

            try
            {
                // Call the product service to update the product details in the database
                var result = await _productService.UpdateProductAsync(updatedProductDto);

                // If the product does not exist, return a NotFound response
                if (!result)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found for update.", updatedProductDto.ProductId);
                    return NotFound(); // Return 404 Not Found if the product was not found in the database
                }

                // Log the successful update of the product
                _logger.LogInformation("Product with ID: {ProductId} updated successfully.", updatedProductDto.ProductId);

                // Return NoContent (204) if the update was successful and there’s no content to return
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception in case of an error during the update process
                _logger.LogError(ex, "Error occurred while updating product with ID: {ProductId}", updatedProductDto.ProductId);

                // Return a 500 Internal Server Error if an exception occurs
                return StatusCode(500, "Internal server error while updating the product");
            }
        }


        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="newProductDto">The product details to add.</param>
        /// <returns>Returns 201 Created with the new product's details; otherwise, returns 500 on error.</returns>
        [Authorize(Roles = "Editor")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreateDto newProductDto)
        {
            // Validate the incoming data using the model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed. Returning BadRequest.");
                return BadRequest(ModelState); // Return a bad request response if the model validation fails
            }

            _logger.LogInformation("Attempting to add a new product.");

            try
            {
                // Add the new product by calling the product service
                var product = await _productService.AddProductAsync(newProductDto);

                // Log successful product addition
                _logger.LogInformation("Product added successfully with ID: {ProductId}", product.ProductId);

                // Return a 201 Created response with the newly created product details
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, new ProductDto
                {
                    ProductId = product.ProductId,
                    InStock = product.InStock,
                    ArrivalDate = product.ArrivalDate,
                    ProductName = newProductDto.ProductName // Return ProductName from the incoming DTO
                });
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the product addition process
                _logger.LogError(ex, "Error occurred while adding the new product");

                // Return a 500 Internal Server Error response with a relevant message
                return StatusCode(500, "Internal server error while adding the product");
            }
        }
    }
}
