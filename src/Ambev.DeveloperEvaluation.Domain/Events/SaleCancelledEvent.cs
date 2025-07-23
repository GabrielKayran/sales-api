namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent
{
    public Guid SaleId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime CancelledAt { get; set; }
    public string Reason { get; set; } = string.Empty;
} 