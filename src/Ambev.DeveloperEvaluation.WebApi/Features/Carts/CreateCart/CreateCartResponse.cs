namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductResponseDto> Products { get; set; } = new List<CreateCartProductResponseDto>();
    public DateTime CreatedAt { get; set; }
}

public class CreateCartProductResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
