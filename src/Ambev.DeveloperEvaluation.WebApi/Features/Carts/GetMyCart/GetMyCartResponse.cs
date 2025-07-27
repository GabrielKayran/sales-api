namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetMyCart;

public class GetMyCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetMyCartProductResponseDto> Products { get; set; } = new List<GetMyCartProductResponseDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetMyCartProductResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
