namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductResult
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public CreateProductRatingResult Rating { get; set; } = new CreateProductRatingResult();
    public DateTime CreatedAt { get; set; }
}

public class CreateProductRatingResult
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
