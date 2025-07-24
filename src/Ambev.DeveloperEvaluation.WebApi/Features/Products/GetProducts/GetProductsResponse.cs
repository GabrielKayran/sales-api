namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsResponse
{
    public List<GetProductsResponseDto> Data { get; set; } = new List<GetProductsResponseDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class GetProductsResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public GetProductsRatingResponseDto Rating { get; set; } = new GetProductsRatingResponseDto();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetProductsRatingResponseDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
