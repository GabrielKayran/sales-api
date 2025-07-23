namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleRequest
{
    public Guid Id { get; set; }
    public string Reason { get; set; } = string.Empty;
} 