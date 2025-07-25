namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsResponse
{
    public List<GetCartsResponseDto> Data { get; set; } = new List<GetCartsResponseDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class GetCartsResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartsProductResponseDto> Products { get; set; } = new List<GetCartsProductResponseDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetCartsProductResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
