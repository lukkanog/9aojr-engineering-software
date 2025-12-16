using ProductProcessing.Data;
using ProductProcessing.Models;

namespace ProductProcessing.Services;

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

    public async Task<(decimal originalPrice, decimal discountedPrice)> CalculateDiscountedPriceAsync(int id, decimal discountPercentage)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        var discountedPrice = product.CalculateDiscountedPrice(discountPercentage);
        return (product.Price, discountedPrice);
    }
}
