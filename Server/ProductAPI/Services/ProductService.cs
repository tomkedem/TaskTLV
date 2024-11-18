using ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Entities;
using ProductAPI.Interfaces;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductDetails)  // Ensure ProductDetails are loaded
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    InStock = p.InStock,
                    DateAdded = p.DateAdded,
                    ArrivalDate = p.ArrivalDate,
                    ProductName = p.ProductDetails != null ? p.ProductDetails.ProductName : null  // Use conditional expression instead of ?. operator
                })
                .ToListAsync();
        }




        // Method to retrieve a product by ID and return it as a ProductDto
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            // Include ProductDetails when fetching Product
            var product = await _context.Products
                .Include(p => p.ProductDetails) // Ensure ProductDetails is loaded
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return null;

            // Map Product entity to ProductDto, including ProductName
            return new ProductDto
            {
                ProductId = product.ProductId,
                InStock = product.InStock,
                ArrivalDate = product.ArrivalDate,
                DateAdded = product.DateAdded,
                ProductName = product.ProductDetails?.ProductName // Map ProductName from ProductDetails
            };
        }


        // Method to update product details
        public async Task<bool> UpdateProductAsync(ProductUpdateDto updatedProductDto)
        {
            var product = await _context.Products.FindAsync(updatedProductDto.ProductId);
            if (product == null) return false;

            // Update fields selectively

            product.InStock = updatedProductDto.InStock;
            product.ArrivalDate = updatedProductDto.ArrivalDate;

            // Save changes to database
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }



        // Method to add a new product and product details
        public async Task<Product> AddProductAsync(ProductCreateDto newProductDto)
        {
            // First, create and save a new ProductDetails entity
            var newProductDetails = new ProductDetails
            {
                ProductName = newProductDto.ProductName // Set the product name in ProductDetails
            };

            // Add ProductDetails to the context and save to generate ProductCode
            _context.ProductDetails.Add(newProductDetails);
            await _context.SaveChangesAsync(); // ProductCode is now generated and available

            // Create a new Product entity and link it to ProductDetails using ProductCode as ProductId
            var newProduct = new Product
            {
                ProductDetailsId = newProductDetails.ProductCode, // Use ProductCode from ProductDetails
                InStock = newProductDto.InStock,
                ArrivalDate = newProductDto.ArrivalDate
            };

            // Add the new product to the database context
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            // Return the newly created product entity with generated ProductId
            return newProduct;
        }




    }
}
