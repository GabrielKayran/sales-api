using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ProductRating Rating { get; set; } = new ProductRating();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProduct(string title, decimal price, string description, string category, string image, ProductRating? rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        if (rating != null)
            Rating = rating;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class ProductRating
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
