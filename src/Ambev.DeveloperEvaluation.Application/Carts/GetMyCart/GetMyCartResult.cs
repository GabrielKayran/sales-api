namespace Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;

public class GetMyCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetMyCartProductResult> Products { get; set; } = new List<GetMyCartProductResult>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetMyCartProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
