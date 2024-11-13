using Microsoft.EntityFrameworkCore;
using ProductAPI.Entities;
using ProductAPI.Entities.ProductAPI.Entities;

namespace ProductAPI.Data // Make sure the namespace matches the folder structure
{
    // ApplicationDbContext represents the session between the application and the database.
    // It is responsible for querying and saving instances of the entities to the database.
    public class ApplicationDbContext : DbContext
    {
        // DbSet for the 'Users' table - represents all the users in the database.
        public DbSet<User> Users { get; set; }

        // DbSet for the 'Products' table - represents all the products in the database.
        public DbSet<Product> Products { get; set; }

        // DbSet for the 'ProductDetails' table - represents product details in the database.
        public DbSet<ProductDetails> ProductDetails { get; set; }

        // Constructor to pass options for DbContext initialization
        // The options contain database connection information like the connection string.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
