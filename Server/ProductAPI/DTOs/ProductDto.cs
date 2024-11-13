public class ProductDto
{
    public int ProductId { get; set; }
    public bool InStock { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime DateAdded { get; set; }
    public string ProductName { get; set; } // Added ProductName
}
