namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<UpdateCartProductDto> Products { get; set; } = new List<UpdateCartProductDto>();
}

public class UpdateCartProductDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
