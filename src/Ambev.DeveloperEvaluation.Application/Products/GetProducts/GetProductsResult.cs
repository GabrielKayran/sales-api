namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsResult
{
    public List<ProductDto> Data { get; set; } = new List<ProductDto>();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ProductRatingDto Rating { get; set; } = new ProductRatingDto();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ProductRatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
