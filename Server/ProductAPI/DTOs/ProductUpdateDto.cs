using System.ComponentModel.DataAnnotations;

public class ProductUpdateDto
{
    // ProductId should always be provided for an update
    [Required(ErrorMessage = "Product ID is required for update.")]
    public int ProductId { get; set; }

    // InStock should be provided to indicate whether the product is in stock
    [Required(ErrorMessage = "InStock status is required.")]
    public bool InStock { get; set; }

    // ArrivalDate can be null for updates, but if provided, it should be a valid date
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]   
    public DateTime? ArrivalDate { get; set; }
}
