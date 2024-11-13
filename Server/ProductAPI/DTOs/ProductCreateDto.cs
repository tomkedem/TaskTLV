using System.ComponentModel.DataAnnotations;

public class ProductCreateDto
{
    // InStock should be provided to indicate whether the product is in stock
    [Required(ErrorMessage = "InStock status is required.")]
    public bool InStock { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime? ArrivalDate { get; set; }

    // ProductName should be required and have a string length limitation
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(20, ErrorMessage = "Product name cannot exceed 20 characters.")]
    public string ProductName { get; set; } // This is for ProductDetails

   
}
