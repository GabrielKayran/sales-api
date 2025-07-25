namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<UpdateCartProductResult> Products { get; set; } = new List<UpdateCartProductResult>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateCartProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
