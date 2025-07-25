using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartCommand : IRequest<CreateCartResult>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductCommand> Products { get; set; } = new List<CreateCartProductCommand>();
}

public class CreateCartProductCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
