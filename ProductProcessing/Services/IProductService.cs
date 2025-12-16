using ProductProcessing.Models;

namespace ProductProcessing.Services;

/// <summary>
/// Service interface - GRASP Low Coupling principle
/// Defines business operations contract
/// </summary>
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<decimal> CalculateDiscountedPriceAsync(int id, decimal discountPercentage);
}
