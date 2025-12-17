using NSubstitute;
using ProductProcessing.Data;
using ProductProcessing.Models;
using ProductProcessing.Services;
using Xunit;

namespace ProductProcessing.Tests.Unit.Services;

public class ProductServiceTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(_repository);
    }

    [Fact]
    public async Task When_HasProducts_ReturnAll()
    {
        var products = new List<Product> { new() { Id = 1 }, new() { Id = 2 } };
        _repository.GetAllAsync().Returns(Task.FromResult<IEnumerable<Product>>(products));

        var result = await _service.GetAllProductsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task When_HasExistingId_ReturnProduct()
    {
        var product = new Product { Id = 1 };
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(product));

        var result = await _service.GetProductByIdAsync(1);

        Assert.Same(product, result);
    }

    [Fact]
    public async Task When_HasMissingId_ReturnNull()
    {
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(null));

        var result = await _service.GetProductByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task When_CreateWithInvalidProduct_ThrowAndDoNotPersist()
    {
        var product = new Product { Name = null, Price = -1, Stock = -1 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateProductAsync(product));
        await _repository.DidNotReceive().CreateAsync(Arg.Any<Product>());
    }

    [Fact]
    public async Task When_CreateWithValidProduct_PersistAndReturn()
    {
        var product = new Product { Name = "Valid", Price = 1m, Stock = 1 };
        var created = new Product { Id = 10 };
        _repository.CreateAsync(product).Returns(Task.FromResult(created));

        var result = await _service.CreateProductAsync(product);

        Assert.Equal(10, result.Id);
    }

    [Fact]
    public async Task When_UpdateMissingProduct_ThrowNotFound()
    {
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(null));

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateProductAsync(1, new Product()));

        Assert.Contains("not found", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task When_UpdateWithInvalidProduct_ThrowAndDoNotUpdate()
    {
        var existing = new Product { Id = 1, CreatedAt = DateTime.UtcNow };
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(existing));

        var product = new Product { Name = null, Price = -1, Stock = -1 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateProductAsync(1, product));
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Product>());
    }

    [Fact]
    public async Task When_UpdateWithValidProduct_PreserveCreatedAtAndUpdate()
    {
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var existing = new Product { Id = 1, CreatedAt = createdAt };
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(existing));

        var product = new Product { Name = "Valid", Price = 1m, Stock = 1 };

        var updated = new Product { Id = 1, CreatedAt = createdAt };
        _repository.UpdateAsync(Arg.Any<Product>()).Returns(Task.FromResult(updated));

        var result = await _service.UpdateProductAsync(1, product);

        Assert.Equal(createdAt, result.CreatedAt);
        await _repository.Received(1).UpdateAsync(Arg.Is<Product>(p => p.Id == 1 && p.CreatedAt == createdAt));
    }

    [Fact]
    public async Task When_DeleteById_ReturnRepositoryResult()
    {
        _repository.DeleteAsync(1).Returns(Task.FromResult(true));

        var result = await _service.DeleteProductAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task When_DiscountMissingProduct_ThrowNotFound()
    {
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CalculateDiscountedPriceAsync(1, 10));
    }

    [Fact]
    public async Task When_DiscountExistingProduct_ReturnOriginalAndDiscounted()
    {
        var product = new Product { Price = 100m };
        _repository.GetByIdAsync(1).Returns(Task.FromResult<Product?>(product));

        var (original, discounted) = await _service.CalculateDiscountedPriceAsync(1, 10m);

        Assert.Equal(100m, original);
        Assert.Equal(90m, discounted);
    }
}
