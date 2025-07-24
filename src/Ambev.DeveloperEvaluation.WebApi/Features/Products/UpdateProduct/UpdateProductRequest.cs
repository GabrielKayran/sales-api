namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public UpdateProductRatingDto Rating { get; set; } = new UpdateProductRatingDto();
}

public class UpdateProductRatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
