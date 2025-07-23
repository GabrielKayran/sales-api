using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleCommand : IRequest<CancelSaleResponse>
{
    public Guid Id { get; set; }
    public string Reason { get; set; } = string.Empty;

    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
} 