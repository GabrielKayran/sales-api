namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public class GetCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartProductResponseDto> Products { get; set; } = new List<GetCartProductResponseDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetCartProductResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
