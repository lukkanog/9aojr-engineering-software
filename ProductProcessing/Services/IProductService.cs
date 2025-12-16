using ProductProcessing.Models;

namespace ProductProcessing.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<(decimal originalPrice, decimal discountedPrice)> CalculateDiscountedPriceAsync(int id, decimal discountPercentage);
}
