using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Entities;
using BCrypt.Net;
using ProductAPI.Entities.ProductAPI.Entities;  // Add the BCrypt library for password hashing

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedDataAsync()
    {
        // Seed ProductDetails data
        if (!await _context.ProductDetails.AnyAsync())
        {
            var productDetailsData = await File.ReadAllTextAsync("SeedData/ProductDetailsSeedData.json");
            var productDetailsList = JsonSerializer.Deserialize<List<ProductDetails>>(productDetailsData);

            if (productDetailsList != null)
            {
                _context.ProductDetails.AddRange(productDetailsList);
                await _context.SaveChangesAsync();
            }
        }

        // Seed Products data
        if (!await _context.Products.AnyAsync())
        {
            var productData = await File.ReadAllTextAsync("SeedData/ProductSeedData.json");
            var productList = JsonSerializer.Deserialize<List<Product>>(productData);

            if (productList != null)
            {
                _context.Products.AddRange(productList);
                await _context.SaveChangesAsync();
            }
        }

        // Seed Users data (with hashed passwords)
        if (!await _context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Password = HashPassword("admin123"),  // Hashed password
                    Role = "Editor"  // Role for Admin
                },
                new User
                {
                    Username = "viewer",
                    Password = HashPassword("viewer123"),  // Hashed password
                    Role = "Viewer"  // Role for Viewer
                }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
        }
    }

    // Method to hash passwords securely using BCrypt
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);  // Use BCrypt for hashing passwords
    }
}
