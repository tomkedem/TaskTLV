using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Entities;

public class ProductDetails
{
    // Specify the primary key
    [Key]
    public int ProductCode { get; set; }

    public string ProductName { get; set; }
}
