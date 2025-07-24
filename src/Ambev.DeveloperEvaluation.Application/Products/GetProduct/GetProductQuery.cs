using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductQuery : IRequest<GetProductResult>
{
    public Guid Id { get; set; }

    public GetProductQuery(Guid id)
    {
        Id = id;
    }
}
