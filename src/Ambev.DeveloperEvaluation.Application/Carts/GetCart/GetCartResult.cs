namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartProductResult> Products { get; set; } = new List<GetCartProductResult>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetCartProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
