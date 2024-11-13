using Xunit;
using Moq;
using ProductAPI.Services;
using ProductAPI.Entities;
using ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductAPI.DTOs;

namespace ProductAPI.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Creates an in-memory database context for testing purposes.
        /// </summary>
        /// <returns>An instance of ApplicationDbContext configured to use an in-memory database.</returns>
        private ApplicationDbContext GetInMemoryDbContext()
        {
            // Configure the DbContextOptions to use an in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Assign a unique name for the test database
                .Options;

            // Return a new instance of ApplicationDbContext with the in-memory database options
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProductsWithDetails()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Clear existing data to ensure a fresh test state
            context.ProductDetails.RemoveRange(context.ProductDetails);
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();

            // Add unique ProductDetails entries
            var productDetails1 = new ProductDetails { ProductCode = 1001, ProductName = "Product A" };
            var productDetails2 = new ProductDetails { ProductCode = 1002, ProductName = "Product B" };
            var productDetails3 = new ProductDetails { ProductCode = 1003, ProductName = "Product C" };

            context.ProductDetails.AddRange(productDetails1, productDetails2, productDetails3);

            // Add corresponding Product entries
            var product1 = new Product { ProductId = 10, InStock = true, ArrivalDate = DateTime.UtcNow, ProductDetails = productDetails1 };
            var product2 = new Product { ProductId = 20, InStock = false, ArrivalDate = DateTime.UtcNow, ProductDetails = productDetails2 };
            var product3 = new Product { ProductId = 30, InStock = true, ArrivalDate = DateTime.UtcNow, ProductDetails = productDetails3 };

            context.Products.AddRange(product1, product2, product3);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllProductsAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, p => p.ProductName == "Product A");
            Assert.Contains(result, p => p.ProductName == "Product B");
            Assert.Contains(result, p => p.ProductName == "Product C");
        }





        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange: set up in-memory database context and initialize service
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Add ProductDetails entity to the context
            var productDetails = new ProductDetails { ProductName = "Sample Product" };
            context.ProductDetails.Add(productDetails);
            await context.SaveChangesAsync();

            // Add the Product without manually setting ProductId (let it auto-generate)
            var product = new Product
            {
                InStock = true,
                ArrivalDate = DateTime.UtcNow,
                ProductDetails = productDetails
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act: retrieve the product by its ID
            var result = await service.GetProductByIdAsync(product.ProductId); // Use the actual ID of the saved product

            // Assert: verify that the product was retrieved and matches the expected values
            Assert.NotNull(result);
            Assert.Equal(product.ProductId, result.ProductId); // Compare to the auto-generated ID
            Assert.Equal("Sample Product", result.ProductName); // Ensure ProductName is mapped correctly
        }




        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange: set up in-memory database context and service
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Act: attempt to retrieve a product with an ID that doesn't exist in the DbContext
            var result = await service.GetProductByIdAsync(99);

            // Assert: verify that the result is null, indicating product not found
            Assert.Null(result);
        }


        [Fact]
        public async Task AddProductAsync_ShouldAddProductSuccessfully()
        {
            // Arrange: set up in-memory database context and service
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Create a ProductCreateDto object
            var newProductDto = new ProductCreateDto
            {
                ProductName = "Test Product",
                InStock = true,
                ArrivalDate = DateTime.UtcNow
            };

            // Act: add the new product
            var result = await service.AddProductAsync(newProductDto);

            // Assert: verify the product was added successfully
            Assert.NotNull(result); // Ensure the result is not null
            Assert.True(result.ProductId > 0); // Check that ProductId is greater than 0
            Assert.Equal(newProductDto.InStock, result.InStock); // Ensure InStock property matches
        }



        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProductDetails_WhenProductExists()
        {
            // Arrange: set up in-memory database context and service
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Create an existing product and add it to the DbContext
            var existingProduct = new Product { ProductId = 1, InStock = true, ArrivalDate = DateTime.UtcNow };
            context.Products.Add(existingProduct);
            await context.SaveChangesAsync();

            // Create ProductUpdateDto with updated values
            var updatedProductDto = new ProductUpdateDto { ProductId = 1, InStock = false, ArrivalDate = existingProduct.ArrivalDate };

            // Act: update the product using the service
            var result = await service.UpdateProductAsync(updatedProductDto);

            // Assert: check that the update was successful
            Assert.True(result); // Ensure the result indicates success
            var product = await context.Products.FindAsync(1); // Retrieve updated product from context
            Assert.NotNull(product); // Ensure the product is found
            Assert.False(product.InStock); // Verify the InStock property was updated correctly
        }


        [Fact]
        public async Task UpdateProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange: set up in-memory database context and service
            using var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Create ProductUpdateDto for a product that does not exist in the DbContext
            var updatedProductDto = new ProductUpdateDto { ProductId = 99, InStock = false };

            // Act: attempt to update the non-existent product
            var result = await service.UpdateProductAsync(updatedProductDto);

            // Assert: verify that the update result is false, indicating product not found
            Assert.False(result);
        }

    }
}
