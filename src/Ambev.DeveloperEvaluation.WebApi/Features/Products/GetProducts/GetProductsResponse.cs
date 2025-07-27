using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsResponse : PaginatedList<GetProductsResponseDto>
{
    public GetProductsResponse(List<GetProductsResponseDto> items, int count, int pageNumber, int pageSize) 
        : base(items, count, pageNumber, pageSize)
    {
    }
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
