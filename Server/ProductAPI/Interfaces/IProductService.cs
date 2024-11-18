using ProductAPI.Entities;

namespace ProductAPI.Interfaces;
public interface IProductService
{
    Task<Product> AddProductAsync(ProductCreateDto newProductDto);
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<bool> UpdateProductAsync(ProductUpdateDto updatedProductDto);
}