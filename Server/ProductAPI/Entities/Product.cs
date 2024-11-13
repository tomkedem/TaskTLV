namespace ProductAPI.Entities;

public class Product
{
    // Primary key
    public int ProductId { get; set; }

    // Foreign key linking to ProductDetails
    public int ProductDetailsId { get; set; } // Link to ProductDetails

    // Date the product was added to the database
    public DateTime DateAdded { get; set; } = DateTime.UtcNow; // Default to current date and time

    // Indicates if the product is in stock
    public bool InStock { get; set; }

    // Date the product arrived in stock (nullable)
    public DateTime? ArrivalDate { get; set; }

    // Navigation property to ProductDetails
    public ProductDetails ProductDetails { get; set; }
}
