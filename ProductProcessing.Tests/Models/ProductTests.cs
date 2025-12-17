using ProductProcessing.Models;
using Xunit;

namespace ProductProcessing.Tests.Unit.Models;

public class ProductTests
{
    [Theory]
    [InlineData("Name", 0, 0, true)]
    [InlineData("Name", 10, 5, true)]
    [InlineData("", 10, 5, false)]
    [InlineData(" ", 10, 5, false)]
    [InlineData(null, 10, 5, false)]
    [InlineData("Name", -1, 5, false)]
    [InlineData("Name", 10, -1, false)]
    public void IsValid_ReturnsExpected(string? name, decimal price, int stock, bool expected)
    {
        var product = new Product
        {
            Name = name ?? string.Empty,
            Price = price,
            Stock = stock
        };

        var result = product.IsValid();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(100, 0, 100)]
    [InlineData(100, 10, 90)]
    [InlineData(100, 50, 50)]
    [InlineData(100, 100, 0)]
    public void CalculateDiscountedPrice_ValidPercentage_ReturnsExpected(decimal price, decimal discountPercentage, decimal expected)
    {
        var product = new Product { Price = price };

        var result = product.CalculateDiscountedPrice(discountPercentage);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void CalculateDiscountedPrice_InvalidPercentage_Throws(decimal discountPercentage)
    {
        var product = new Product { Price = 100m };

        Assert.Throws<ArgumentException>(() => product.CalculateDiscountedPrice(discountPercentage));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsAvailable_ReturnsExpected(int stock, bool expected)
    {
        var product = new Product { Stock = stock };

        var result = product.IsAvailable();

        Assert.Equal(expected, result);
    }
}
