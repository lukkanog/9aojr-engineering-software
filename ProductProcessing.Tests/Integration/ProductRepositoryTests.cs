using Microsoft.EntityFrameworkCore;
using ProductProcessing.Data;
using ProductProcessing.Models;
using Xunit;

namespace ProductProcessing.Tests.Integration;

public class ProductRepositoryTests
{
    private static ProductDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: $"ProductDb_{Guid.NewGuid()}")
            .Options;

        return new ProductDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_SetsCreatedAtAndPersists()
    {
        await using var context = CreateInMemoryContext();
        var repository = new ProductRepository(context);
        var product = new Product { Name = "Test", Price = 10m, Stock = 5 };

        var created = await repository.CreateAsync(product);

        Assert.NotEqual(default, created.CreatedAt);
        Assert.Equal(1, await context.Products.CountAsync());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        await using var context = CreateInMemoryContext();
        context.Products.Add(new Product { Name = "P1", Price = 1m, Stock = 1, CreatedAt = DateTime.UtcNow });
        context.Products.Add(new Product { Name = "P2", Price = 2m, Stock = 2, CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();
        var repository = new ProductRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTrackedCopyWithoutTracking()
    {
        await using var context = CreateInMemoryContext();
        var product = new Product { Name = "P1", Price = 1m, Stock = 1, CreatedAt = DateTime.UtcNow };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var repository = new ProductRepository(context);

        var loaded = await repository.GetByIdAsync(product.Id);

        Assert.NotNull(loaded);
        Assert.Equal(product.Id, loaded!.Id);
    }

    [Fact]
    public async Task UpdateAsync_SetsUpdatedAtAndPersists()
    {
        await using var context = CreateInMemoryContext();
        var product = new Product { Name = "P1", Price = 1m, Stock = 1, CreatedAt = DateTime.UtcNow };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var repository = new ProductRepository(context);

        product.Price = 5m;
        var updated = await repository.UpdateAsync(product);

        Assert.Equal(5m, updated.Price);
        Assert.NotNull(updated.UpdatedAt);

        var reloaded = await context.Products.FindAsync(product.Id);
        Assert.Equal(5m, reloaded!.Price);
        Assert.NotNull(reloaded.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProductAndReturnsTrue_WhenExists()
    {
        await using var context = CreateInMemoryContext();
        var product = new Product { Name = "P1", Price = 1m, Stock = 1, CreatedAt = DateTime.UtcNow };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var repository = new ProductRepository(context);

        var result = await repository.DeleteAsync(product.Id);

        Assert.True(result);
        Assert.Equal(0, await context.Products.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotExists()
    {
        await using var context = CreateInMemoryContext();
        var repository = new ProductRepository(context);

        var result = await repository.DeleteAsync(123);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenProductExists()
    {
        await using var context = CreateInMemoryContext();
        var product = new Product { Name = "P1", Price = 1m, Stock = 1, CreatedAt = DateTime.UtcNow };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var repository = new ProductRepository(context);

        var exists = await repository.ExistsAsync(product.Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenProductDoesNotExist()
    {
        await using var context = CreateInMemoryContext();
        var repository = new ProductRepository(context);

        var exists = await repository.ExistsAsync(999);

        Assert.False(exists);
    }
}
