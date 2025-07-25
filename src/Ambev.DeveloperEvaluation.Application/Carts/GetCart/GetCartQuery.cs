using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartQuery : IRequest<GetCartResult>
{
    public Guid Id { get; }

    public GetCartQuery(Guid id)
    {
        Id = id;
    }
}
