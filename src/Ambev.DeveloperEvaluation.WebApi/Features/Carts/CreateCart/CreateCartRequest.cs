namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartRequest
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductDto> Products { get; set; } = new List<CreateCartProductDto>();
}

public class CreateCartProductDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
