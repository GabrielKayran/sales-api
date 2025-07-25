using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartCommand : IRequest<bool>
{
    public Guid Id { get; }

    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }
}
