using ProductProcessing.Models;

namespace ProductProcessing.Data;

/// <summary>
/// Repository interface - GRASP Low Coupling and Polymorphism principles
/// Abstracts data access operations to reduce coupling with concrete implementations
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
