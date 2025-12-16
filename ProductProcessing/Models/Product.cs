namespace ProductProcessing.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && Price >= 0 && Stock >= 0;
    }

    public decimal CalculateDiscountedPrice(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100");

        return Price * (1 - discountPercentage / 100);
    }

    public bool IsAvailable()
    {
        return Stock > 0;
    }
}
