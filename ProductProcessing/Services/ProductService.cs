using ProductProcessing.Data;
using ProductProcessing.Models;

namespace ProductProcessing.Services;

/// <summary>
/// Product service - GRASP High Cohesion and Controller principles
/// High Cohesion: All business logic related to products is in one place
/// Controller: Coordinates operations between repository and presentation layer
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Business logic validation
        if (!product.IsValid())
        {
            throw new InvalidOperationException("Product validation failed");
        }

        return await _repository.CreateAsync(product);
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _repository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        // Business logic validation
        if (!product.IsValid())
        {
            throw new InvalidOperationException("Product validation failed");
        }

        product.Id = id;
        product.CreatedAt = existingProduct.CreatedAt;
        return await _repository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<decimal> CalculateDiscountedPriceAsync(int id, decimal discountPercentage)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        // Delegating to the Information Expert (Product knows how to calculate its own discount)
        return product.CalculateDiscountedPrice(discountPercentage);
    }
}
