namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsResult
{
    public List<GetCartsResultDto> Data { get; set; } = new List<GetCartsResultDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class GetCartsResultDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartsProductResultDto> Products { get; set; } = new List<GetCartsProductResultDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetCartsProductResultDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
