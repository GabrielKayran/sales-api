using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale is cancelled
/// </summary>
public class SaleCancelledEvent : INotification
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public DateTime CancelledAt { get; }
    public string Reason { get; }
    public decimal CancelledAmount { get; }

    public SaleCancelledEvent(Guid saleId, string saleNumber, string reason, decimal cancelledAmount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        Reason = reason;
        CancelledAmount = cancelledAmount;
        CancelledAt = DateTime.UtcNow;
    }
} 