namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductResult> Products { get; set; } = new List<CreateCartProductResult>();
    public DateTime CreatedAt { get; set; }
}

public class CreateCartProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
